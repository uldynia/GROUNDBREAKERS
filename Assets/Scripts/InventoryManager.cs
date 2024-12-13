using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class InventoryManager : MonoBehaviour
{
    [SerializeField] public Transform InventoryItemDisplayTransform;
    [SerializeField] public GameObject p_InventoryItemDisplay;
    public static InventoryManager instance;
    public Dictionary<string, Item> items = new();
    void Start() {
        instance = this;
        foreach(var item in Resources.LoadAll<Item>("Items")) {
            items.Add(item.name, item);
        }
    }
    public void ShowItems(List<(Item, int)> items) {
        foreach(Transform child in InventoryItemDisplayTransform) {
            Destroy(child.gameObject);
        }
        foreach(var item in items) {
            Instantiate(p_InventoryItemDisplay, InventoryItemDisplayTransform).GetComponent<InventoryItemDisplay>().Init(item);
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

    public static void WriteInventoryItem(this NetworkWriter writer, (Item, int) value) {
        writer.Write<string>(value.Item1.name);
        writer.Write<int>(value.Item2);
    }
    public static (Item, int) ReadInventoryItem(this NetworkReader reader) {
        var i = (InventoryManager.instance.items[reader.Read<string>()], reader.Read<int>());
        Debug.Log(i);
        return i;
    }
}