using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Tilemaps;
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Dictionary<string, Item> items = new();
    void Start() {
        instance = this;
        foreach(var item in Resources.LoadAll<Item>("Items")) {
            items.Add(item.name, item);
        }
    }
}
public static class ItemReadWriter
{
    public static void WriteItem(this NetworkWriter writer, Item value)
    {
        writer.Write<string>(value.name);
    }

    public static Item ReadItem(this NetworkReader reader)
    {
        return InventoryManager.instance.items[reader.ReadString()];
    }
}