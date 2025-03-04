using LightReflectiveMirror;
using Mirror;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Izumo : NetworkBehaviour // the lobby. Takama is the main game
{
    [SyncVar(hook = nameof(connect))] public int playerCount, playerReady = 0;
    [SyncVar] public float startCooldown;
    [SyncVar] public string countText;

    [SerializeField] CanvasGroup lobbyDisplay, loadingScreen;
    [SerializeField] TextMeshProUGUI playerCountText;
    [SerializeField] Takama p_takama;
    string id;
    private void Start()
    {
        Loading.instance.SetDoor(true);
        var transport = NetworkManager.instance.transport as LightReflectiveMirrorTransport;
        id = transport.serverId;
    }
    // Update is called once per frame
    void Update()
    {
        playerCountText.text = countText;
        if (PlayerController.instance == null || Takama.instance != null)
        {
            playerCountText.text = "";
            return;
        }
        if (isServer)
        {
            playerCount = NetworkServer.connections.Count;
            countText = $"Server code: {id}\nPlayers Ready: {playerReady / 2}/{playerCount}";

        }
        else return;
        if (playerCount == (playerReady/2) && MissionsManager.instance.currentMission != null)
        {
            countText += $"\nStarting in: {(int)startCooldown}";
            startCooldown -= Time.deltaTime;
            if (startCooldown < 0) // start game
            {
                var takama = Instantiate(p_takama);
                NetworkServer.Spawn(takama.gameObject);
                takama.Init(1200, 300, 30);
            }
        }
        else
        {
            startCooldown = 5;
        }
    }
    void connect(int oldvar, int newvar)
    {
        //Debug.Log("Player connected!");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.instance.cameraTargetSize = 12;
            if (PlayerController.instance.isServer)
            {
                playerReady++;
                Debug.Log($"Player entered the Ready zone. {playerReady}");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
