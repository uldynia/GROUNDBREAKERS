using Mirror;
using UnityEngine;

public class QuantaBullet : NetworkBehaviour
{

    Vector3 direction;
    float damage;
    public void init(Vector3 _direction, float _damage)
    {
        direction = _direction;
        damage = _damage;
    }
    private void Update()
    {
        if(isServer)
        transform.position += direction * Time.deltaTime * 5;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isServer) return;
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<Entity>().Damage(damage);
            NetworkServer.Destroy(gameObject);
        }
    }
}
