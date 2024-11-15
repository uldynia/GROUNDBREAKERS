using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Izumo : NetworkBehaviour // the lobby. Takama is the main game
{
    [SyncVar(hook =nameof(connect))] public int playerCount, playerReady = 0;
    [SyncVar] public float startCooldown;


    [SerializeField] CanvasGroup lobbyDisplay, loadingScreen;
    [SerializeField] TextMeshProUGUI playerCountText;
    [SerializeField] Takama p_takama;
    private void Start()
    {
        if (Application.platform == RuntimePlatform.WindowsServer) { 
            FindFirstObjectByType<NetworkManager>().StartServer();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance == null || !PlayerController.instance.isServer || Takama.instance != null)
        {
            playerCountText.text = "";
            return;
        }

        playerCountText.text = $"Players Ready: {playerReady}/{playerCount}\nStarting in: {(int)startCooldown}";
        playerCount = NetworkServer.connections.Count;
        if(playerCount == playerReady)
        {
            startCooldown -= Time.deltaTime;
            if(startCooldown < 0) // start game
            {
                var takama = Instantiate(p_takama);
                NetworkServer.Spawn(takama.gameObject);
                takama.Init(500, 500, 30);
            }
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
        if(other.CompareTag("Player") )
        {
            PlayerController.instance.cameraTargetSize = 12;
            if(PlayerController.instance.isServer)
            {
                playerReady++;
                Debug.Log($"Player entered the Ready zone. {playerReady}");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") )
        {
            PlayerController.instance.cameraTargetSize = 10;
            if (PlayerController.instance.isServer)
            {
                playerReady--;
                Debug.Log($"Player left the Ready zone. {playerReady}");
            }
        }
    }
}
