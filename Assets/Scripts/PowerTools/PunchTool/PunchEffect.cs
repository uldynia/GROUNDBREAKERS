using Mirror;
using UnityEngine;

public class PunchEffect : NetworkBehaviour
{
    [SyncVar] Vector2 direction;
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
            if (lifespan > 0.15f) NetworkServer.Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (isServer) {

            rb.MovePosition(rb.position + direction * Time.fixedDeltaTime * 25);
        }
    }
    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;
        if(collision.CompareTag("Takama"))
        {
            var hit = Physics2D.Raycast(transform.position, direction, 1, LayerMask.GetMask("Terrain"));
            var destiny = Vector3Int.FloorToInt(new Vector2(hit.point.x, hit.point.y) + direction * 0.5f);
            Takama.instance?.BreakTile(destiny);
            NetworkServer.Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy")) {
            collision.GetComponent<Entity>()?.Damage(5);
        }
    }
}
