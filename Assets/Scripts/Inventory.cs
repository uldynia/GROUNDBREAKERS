using System;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public class Inventory : NetworkBehaviour
{
    public readonly SyncList<(Item, int)> items = new();
    [Server]
    public int AddItem(Item item, int amount)
    {
        if (item == null) throw new NullReferenceException();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Item1 == item)
            {
                items[i] = (item, items[i].Item2 + amount);
                ShowItems(connectionToClient, items.ToList());
                return items[i].Item2;
            }
        }
        items.Add((item, amount));
        ShowItems(connectionToClient, items.ToList());
        return amount;
    }
    [Server]
    public bool RemoveItem(Item item, int amount)
    {
        if (item == null) throw new NullReferenceException();
        var i = items.FindIndex(item => item.Item1);
        if (items[i].Item2 - amount < 0) return false;
        items[i] = (items[i].Item1, items[i].Item2 - amount);
        ShowItems(connectionToClient, items.ToList());
        return true;
    }
    [TargetRpc]
    public void ShowItems(NetworkConnectionToClient target, List<(Item, int)> items)
    {
        InventoryManager.instance.ShowItems(items);
    }
}