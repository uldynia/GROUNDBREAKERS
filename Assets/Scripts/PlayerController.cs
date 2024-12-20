using Mirror;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController instance;
    public static string menuUsername;
    [SyncVar] public string username = "user";
    public Rigidbody2D rb;
    SpriteRenderer sr;
    bool canJump = true;
    public float cameraTargetSize = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 120;
        ControlsManager.instance.jumpButton.onDown += Jump;
        if (isLocalPlayer)
        {
            name = "LocalPlayer";
            SetUsername(menuUsername);
            instance = this;
        }
    }
    [Command]
    void SetUsername(string _name)
    {
        username = _name;
    }
    [SyncVar] bool isFlipped;
    void Update()
    {
        sr.flipX = isFlipped;
        if (!isOwned) return;
        if (ControlsManager.instance.horizontal != 0)
            isFlipped = (ControlsManager.instance.horizontal > 0);
        if ((canJump))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            transform.position += new Vector3(ControlsManager.instance.horizontal * Time.deltaTime * 10, 0);

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
}
