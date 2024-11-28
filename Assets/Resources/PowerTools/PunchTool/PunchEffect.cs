using Mirror;
using UnityEngine;

public class PunchEffect : NetworkBehaviour
{
    [SyncVar] Vector3 direction;
    [SyncVar] float lifespan = 0;
    Rigidbody2D rb;
    public void init(Vector3 direction)
    {
        this.direction = direction;
        transform.up = direction;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(isServer)
        {
            lifespan += Time.deltaTime;
            rb.MovePosition(transform.position + direction * Time.deltaTime * 10);
            if (lifespan > 0.3f) NetworkServer.Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;
        if(collision.CompareTag("Takama"))
        {
            Takama.instance?.SetTile(Vector3Int.RoundToInt(transform.position + direction), 0);
            NetworkServer.Destroy(gameObject);
        }
    }
}
