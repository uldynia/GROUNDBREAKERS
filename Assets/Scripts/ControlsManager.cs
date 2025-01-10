using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class ControlsManager : NetworkBehaviour
{
    public static ControlsManager instance;
    public ArrowButton[] arrowButtons;
    public HoldButton jumpButton;
    [SyncVar] public float horizontal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(isLocalPlayer)
        {
            instance = this;
            arrowButtons[0] = GameObject.Find("Canvas/Controls/Left").GetComponent<ArrowButton>();
            arrowButtons[1] = GameObject.Find("Canvas/Controls/Right").GetComponent<ArrowButton>();
            jumpButton = GameObject.Find("Canvas/Controls/Jump").GetComponent<HoldButton>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            horizontal = 0;
            foreach (var button in arrowButtons)
            {
                if (button.isHolding)
                    horizontal += button.value;
            }
            if (horizontal == 0)
            {
                horizontal = Input.GetAxis("Horizontal");
            }
        }
    }
}
