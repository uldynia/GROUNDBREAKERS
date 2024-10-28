using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    Rigidbody2D rb;
    bool canJump = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Application.targetFrameRate = 120;
    }

    void Update()
    {
        if (!isLocalPlayer) return;
        if ((canJump))
        {
            rb.AddForce(Vector2.down * 20);
            if (Input.GetKeyDown(KeyCode.Space)) {
                rb.AddForce(Vector2.up * 15, ForceMode2D.Impulse);
            }


            if (Mathf.Abs(rb.velocityX) < 5)
                transform.position += new Vector3(Input.GetAxisRaw("Horizontal") * Time.deltaTime * 10, 0);
        }


        Camera.main.transform.position += ((transform.position - new Vector3(0, 0, 10) - Camera.main.transform.position)) * Time.fixedDeltaTime * 5;
    }
}
