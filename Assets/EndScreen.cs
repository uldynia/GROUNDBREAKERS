using Mirror;
using TMPro;
using UnityEngine;
public class EndScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI winMessage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Loading.instance.SetDoor(true);
        winMessage.text = WinLossManager.instance.won ? "Mission complete." : "Mission failed.";
    }
    public void RestartGame()
    {
        NetworkManager.instance.ServerChangeScene("Game");
    }
    public void Disconnect()
    {
        NetworkManager.instance.StopHost();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
