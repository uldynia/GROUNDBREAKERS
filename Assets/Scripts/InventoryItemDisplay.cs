using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class InventoryItemDisplay : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI count;
    public void Init((Item, int) item) {
        icon.sprite = item.Item1.sprite;
        count.text = item.Item2.ToString();
    }
}
