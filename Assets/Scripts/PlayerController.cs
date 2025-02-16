using Mirror;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController instance;
    public static string menuUsername;
    [SerializeField] Sprite[] spritesheet;
    [SerializeField] GameObject revivePrompt, reviveBar;
    [SyncVar] bool isFlipped;
    [SyncVar] public string username = "user";
    [SyncVar] public int sprite = 0;
    [SyncVar] public float revivalPumps = 0;

    public Rigidbody2D rb;
    SpriteRenderer sr;
    bool canJump => Physics2D.Raycast(transform.position, Vector2.down, 1, LayerMask.GetMask("Terrain")).collider != null;
    public float cameraTargetSize = 10;
    Entity e;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        e = GetComponent<Entity>();
        Application.targetFrameRate = 120;
        ControlsManager.instance.jumpButton.onDown += Jump;
        if (isLocalPlayer)
        {
            name = "LocalPlayer";
            SetUsername(menuUsername);
            instance = this;
        }
    }
    void SetUsername(string _name)
    {
        username = _name;
    }
    void Update()
    {
        sr.flipX = isFlipped;
        sr.sprite = spritesheet[sprite];
        revivePrompt.SetActive(e.health < 0 || revivalPumps > 0);
        if(e.health > 0)
        {
            reviveBar.transform.localScale = Vector3.one;
        }
        else
        {
            reviveBar.transform.localScale = new(revivalPumps / 3, 1);
        }
        if(isServer)
        {
            revivalPumps = Mathf.Max(0, revivalPumps-Time.deltaTime * 0.3f);
        }
        if (!isOwned) return;
        if (e.health > 0)
        {
            if (ControlsManager.instance.horizontal != 0)
            {
                isFlipped = (ControlsManager.instance.horizontal > 0);
                sprite = Mathf.RoundToInt(Time.time * 5) % 2;
            }
            else
            {
                sprite = 0;
            }
            transform.position += new Vector3(ControlsManager.instance.horizontal * Time.deltaTime * 10, 0);
            if (canJump)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Jump();
                }

            }
        }
        else
        {
            sprite = 2;
        }

        if (Vector3.SqrMagnitude(Camera.main.transform.position - transform.position) > 20) Camera.main.transform.position = transform.position - new Vector3(0, 0, 10);
        Camera.main.transform.position += ((transform.position - new Vector3(0, 0, 10) - Camera.main.transform.position)) * Time.fixedDeltaTime * 5;
        Camera.main.orthographicSize += Mathf.Clamp((cameraTargetSize - Camera.main.orthographicSize), -0.5f, 0.5f) * Time.deltaTime * 10;
    }
    void FixedUpdate()
    {
        rb.AddForce(Vector2.up * -15);
    }
    public void Jump()
    {
        rb.linearVelocityY = 15;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(isServer)
        {
            Entity entity = collision.gameObject.GetComponent<Entity>();
            if (collision.gameObject.CompareTag("Player") && entity.health < 0)
            {
                var pc = collision.gameObject.GetComponent<PlayerController>();
                pc.revivalPumps++;
                Debug.Log(pc.revivalPumps);
                if (pc.revivalPumps > 2)
                {
                    entity.health = 20;
                }
            }
        }
    }
}
