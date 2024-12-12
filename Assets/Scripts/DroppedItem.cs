using System;
using Mirror;
using UnityEngine;
using UnityEngine.UI;
public class DroppedItem : NetworkBehaviour
{
    public Item item;
    [SerializeField] SpriteRenderer image;
    [ClientRpc]
    public void Init(Item item) {
        image.sprite = item.sprite;
    }
    float time;
    public void Update() {
        transform.localScale += (Vector3.one * 0.8f - transform.localScale) * Time.deltaTime * 5;
        transform.position += Vector3.up * MathF.Sin((time +=Time.deltaTime) * 2) * 0.002f;
    }
    [Server]
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            other.GetComponent<Inventory>().AddItem(item, 1);
            NetworkServer.Destroy(gameObject);

        }
    }
}
