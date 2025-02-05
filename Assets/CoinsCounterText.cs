using TMPro;
using UnityEngine;

public class CoinsCounterText : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        text.text = SaveSystem.variables["coins"];
    }
}
