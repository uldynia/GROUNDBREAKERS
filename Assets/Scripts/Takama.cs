using Mirror;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Takama : NetworkBehaviour // the main game. Izumo is the lobby.
{
    public static Takama instance;
    Tilemap tilemap;
    [SerializeField] List<TileBase> tiles;

    [SyncVar] Vector2Int spawnpoint;
    private void Start()
    {
        Takama.instance = this;
        tilemap = GetComponentInChildren<Tilemap>();
    }
    public void Init(int xsize, int ysize, int missionlength)
    {
        spawnpoint = new Vector2Int(xsize / 10, ysize / 10 * 9);
        StartCoroutine(init());
        IEnumerator init()
        {
            yield return new WaitForSeconds(1);
            BoxFill(Vector3Int.zero, 1, 0, 0, xsize, ysize);
            GenerateCircle(spawnpoint.x, spawnpoint.y, 30);
            TeleportToSpawn();
        }
    }


    public void GenerateCircle(int xCenter, int yCenter, int radius)
    {
        for (int x = xCenter - radius; x <= xCenter + radius; x++)
        {
            for (int y = yCenter - radius * 2; y <= yCenter + radius; y++)
            {
                if ((x - xCenter) * (x - xCenter) + (y - yCenter) * (y - yCenter) <= radius * radius)
                {
                    SetTile(new Vector3Int(x, y), 0);
                }
            }
        }
    }
    [ClientRpc]
    void TeleportToSpawn() => PlayerController.instance.transform.position = new Vector3(spawnpoint.x, spawnpoint.y);
    [ClientRpc]
    public void BoxFill(Vector3Int position, int id, int startX, int startY, int endX, int endY)
    {
        //Debug.Log($"Filling box {startX} {startY} to {endX} {endY}");
        tilemap.BoxFill(position, tiles[id], startX, startY, endX, endY);
    }
    [ClientRpc]
    public void SetTile(Vector3Int position, int id)
    {
        //Debug.Log($"Setting tile at {position}");
        tilemap.SetTile(position, tiles[id]);
    }
}