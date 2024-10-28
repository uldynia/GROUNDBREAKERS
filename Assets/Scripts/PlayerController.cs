using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public static PlayerController instance;
    Rigidbody2D rb;
    SpriteRenderer sr;
    bool canJump = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 120;
        ControlsManager.instance.jumpButton.onDown += Jump;
        instance = this;
        if (isLocalPlayer)
        {
            PowerToolManager.instance.OnPlayerConnected();
        }
    }

    void Update()
    {
        if (ControlsManager.instance.horizontal != 0)
            sr.flipX = (ControlsManager.instance.horizontal > 0);
        if (!isLocalPlayer) return;
        if ((canJump))
        {
            rb.AddForce(Vector2.down * 15);
            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }

            transform.position += new Vector3(ControlsManager.instance.horizontal * Time.deltaTime * 10, 0);
            
        }
        Camera.main.transform.position += ((transform.position - new Vector3(0, 0, 10) - Camera.main.transform.position)) * Time.fixedDeltaTime * 5;
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
    }
}
