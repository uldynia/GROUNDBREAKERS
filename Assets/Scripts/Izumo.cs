using Mirror;
using TMPro;
using UnityEngine;

public class Izumo : NetworkBehaviour // the lobby. Takama is the main game
{
    [SyncVar(hook =nameof(connect))] public int playerCount = 1, playerReady = 0;
    [SyncVar] public float startCooldown;


    [SerializeField] CanvasGroup lobbyDisplay, loadingScreen;
    [SerializeField] TextMeshProUGUI playerCountText;
    private void Start()
    {
        NetworkServer.OnConnectedEvent += (NetworkConnectionToClient client) => playerCount++;
        NetworkServer.OnDisconnectedEvent += (NetworkConnectionToClient client) => playerCount--;
    }
    // Update is called once per frame
    void Update()
    {
        playerCountText.text = $"Players Ready: {playerReady}/{playerCount}\nStarting in: {(int)startCooldown}";
        if (!PlayerController.instance.isServer || Takama.instance != null) return;
        if(playerCount == playerReady)
        {
            startCooldown -= Time.deltaTime;
            
        }
        else
        {
            startCooldown = 5;
        }
    }
    void connect(int oldvar, int newvar)
    {
        Debug.Log("Player connected!");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && PlayerController.instance.isServer)
        {
            playerReady++;
            Debug.Log($"Player entered the Ready zone. {playerReady}");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && PlayerController.instance.isServer)
        {
            playerReady--;
            Debug.Log($"Player left the Ready zone. {playerReady}");
        }
    }
}
