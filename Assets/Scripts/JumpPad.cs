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
        if(other.CompareTag("Player") && armingTime > 2)
        {
            PlayerController.instance.rb.AddForce(Vector3.up * 50, ForceMode2D.Impulse);
        }
    }
}
