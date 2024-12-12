using Mirror;
using UnityEngine;
using UnityEngine.UI;
public class DroppedItem : NetworkBehaviour
{
    public Item item;
    [SerializeField] SpriteRenderer image;
    [Server]
    public void Init(Item item) {
        image.sprite = item.sprite;
    }
    [Server]
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<Inventory>().AddItem(item, 1);
            NetworkServer.Destroy(gameObject);

        }
    }
}
