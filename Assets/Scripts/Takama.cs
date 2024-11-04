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
            LineFill(0, (Vector3Int)spawnpoint, (Vector3Int)(spawnpoint + new Vector2Int(100, -100)), 3);
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
    public void LineFill(int id, Vector3Int start, Vector3Int end, int thickness)
    {
        List<Vector3Int> vectors = new();
        List<TileBase> _tiles = new();

        int dx = Mathf.Abs(end.x - start.x);
        int dy = Mathf.Abs(end.y - start.y);
        int sx = start.x < end.x ? 1 : -1;
        int sy = start.y < end.y ? 1 : -1;
        int err = dx - dy;

        while (true)

        {
            for (int x = -thickness / 2; x <= thickness / 2; x++)
            {
                for (int y = -thickness / 2; y <= thickness / 2; y++)
                {
                    vectors.Add(new Vector3Int(start.x + x, start.y + y, start.z));
                    _tiles.Add(tiles[id]);
                }
            }

            if (start.x == end.x && start.y == end.y)
                break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; start.x += sx; }
            if (e2 < dx) { err += dx; start.y += sy; }
        }

        tilemap.SetTiles(vectors.ToArray(), _tiles.ToArray());
    }
    [ClientRpc]
    public void SetTile(Vector3Int position, int id)
    {
        //Debug.Log($"Setting tile at {position}");
        tilemap.SetTile(position, tiles[id]);
    }
}