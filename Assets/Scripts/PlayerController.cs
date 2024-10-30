using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController instance;
    public Rigidbody2D rb;
    SpriteRenderer sr;
    bool canJump = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 120;
        ControlsManager.instance.jumpButton.onDown += Jump;
        if (isLocalPlayer)
        {
            instance = this;
            PowerToolManager.instance.OnPlayerConnected();
        }
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
            rb.AddForce(Vector2.down * 15);
            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }

            transform.position += new Vector3(ControlsManager.instance.horizontal * Time.deltaTime * 10, 0);
            
        }
        if (Vector3.SqrMagnitude(Camera.main.transform.position - transform.position) > 20) Camera.main.transform.position = transform.position - new Vector3(0, 0, 10);
        Camera.main.transform.position += ((transform.position - new Vector3(0, 0, 10) - Camera.main.transform.position)) * Time.fixedDeltaTime * 5;
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
    }
}
