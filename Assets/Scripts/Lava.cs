using Mirror;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lava : MonoBehaviour
{
    Entity e;
    TileCollider tileCollider;
    bool shouldDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TileCollider>().onCollide += OnCollide;
        e = GetComponent<Entity>();
    }
    public void OnCollide(TileBase tileBase)
    {
        if (tileBase == Takama.instance.tiles[2].tile)
        {
            shouldDamage = true;
        }
    }
    [Server]
    void Update()
    {
        if(shouldDamage) {
            shouldDamage = false;
            e.Damage(10 * Time.deltaTime);
        }
    }
}
