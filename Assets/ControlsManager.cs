using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager instance;
    public ArrowButton[] arrowButtons;
    public HoldButton jumpButton, punchButton;
    public float horizontal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = 0;
        foreach (var button in arrowButtons) {
            if(button.isHolding)
                horizontal += button.value;
        }
        if(horizontal == 0)
        {
            horizontal = Input.GetAxis("Horizontal");
        }
    }
}
