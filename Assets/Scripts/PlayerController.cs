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
    ControlsManager controlsManager;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        controlsManager = GetComponent<ControlsManager>();
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
        if (isLocalPlayer)
        {
            Camera.main.transform.position += ((transform.position - new Vector3(0, 0, 10) - Camera.main.transform.position)) * Time.fixedDeltaTime * 5;
            Camera.main.orthographicSize += Mathf.Clamp((cameraTargetSize - Camera.main.orthographicSize), -0.5f, 0.5f) * Time.deltaTime * 10;
            if (Vector3.SqrMagnitude(Camera.main.transform.position - transform.position) > 20)
                Camera.main.transform.position = transform.position - new Vector3(0, 0, 10);

            if (controlsManager.horizontal != 0)
                isFlipped = (controlsManager.horizontal > 0);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

        }
        if (isServer)
        {
            transform.position += new Vector3(controlsManager.horizontal * Time.deltaTime * 10, 0);
        }
    }
    void FixedUpdate()
    {
        rb.AddForce(Vector2.up * -15);
    }
    [Command]
    public void Jump()
    {
        rb.linearVelocityY = 15;
    }
}
