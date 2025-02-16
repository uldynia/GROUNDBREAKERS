using Mirror;
using UnityEngine;

public class WinLossManager : MonoBehaviour
{
    public bool won;
    public static WinLossManager instance {  get; private set; }
    private void Start()
    {
        instance = this;
    }
}
