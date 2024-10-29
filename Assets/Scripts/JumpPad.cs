using Mirror;
using UnityEngine;

public class JumpPad : NetworkBehaviour
{
    [SyncVar] public float armingTime;
    private void Update()
    {
        armingTime += Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && armingTime > 2)
        {
            other.GetComponent<Rigidbody2D>().velocityY = 40;
        }  
    }
}
