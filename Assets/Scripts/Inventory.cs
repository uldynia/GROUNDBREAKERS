using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    public readonly SyncList<(Item, int)> items = new();
    [Server]
    public void AddItem(Item item, int amount)
    {
        if (item == null) throw new NullReferenceException();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Item1 == item)
            {
                items[i] = (item, items[i].Item2 + amount);
                ShowItems(connectionToClient, items.ToList());
                return;
            }
        }
        items.Add((item, amount));
        ShowItems(connectionToClient, items.ToList());
    }
    [TargetRpc]
    public void ShowItems(NetworkConnectionToClient target, List<(Item, int)> items)
    {
        InventoryManager.instance.ShowItems(items);
    }
}