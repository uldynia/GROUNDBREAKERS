using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Loading.instance.SetDoor(true);
    }
    public void RestartGame()
    {
        NetworkManager.instance.ServerChangeScene("Game");
    }
}
