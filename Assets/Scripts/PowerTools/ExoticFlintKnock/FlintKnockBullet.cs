using Mirror;
using UnityEngine;

public class FlintKnockBullet : NetworkBehaviour
{
    [SerializeField] float speed = 15, damage = 250;
    Rigidbody2D rb;
    Vector3 direction;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Init(Vector3 _direction)
    {
        transform.right = _direction;
        direction = _direction;
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = direction;
    }
    [Server]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Entity>().Damage(damage);
        }
        if(collision.CompareTag("Takama"))
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
