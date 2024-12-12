using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    [SyncVar] public List<InventoryItem> items = new();
    [Server]
    public void AddItem(Item item, int amount) {
        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].item == item) {
                items[i].count += amount;
                return;
            }
        }
        items.Add(new InventoryItem(){count = amount, item = item});
    }
}
[System.Serializable]
public class InventoryItem {
    public int count;
    public Item item;
}