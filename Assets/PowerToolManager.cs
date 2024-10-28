using UnityEngine;

public class PowerToolManager : MonoBehaviour
{
    public static PowerToolManager instance;
    [SerializeField] PowerToolButton powerTool1, powerTool2;
    private void Awake()
    {
        instance = this;
    }
    public void OnPlayerConnected()
    {
        PlayerController.instance.GetComponent<JumpPadPowerTool>().Init(powerTool1);
    }
}
