using Mirror;
using System;
using UnityEngine;

[RequireComponent(typeof(TileRenderer))]
public class Takama : NetworkBehaviour // the main game. Izumo is the lobby.
{
    public static Takama instance;
    public Tile[,] tiles;
    Tuple<int,int> spawnpoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
    }
    public void Init(int xsize, int ysize, int missionLength) // mission length is the number of paths generated.
    {
        tiles = new Tile[xsize, ysize];
        spawnpoint = new(xsize / 10, ysize * 9 / 10);



        TeleportToSpawn();
    }
    [Command]
    public void Break(short damage, int x, int y)
    {
        tiles[x,y].breakProgress += damage;
    }

    [ClientRpc]
    public void TeleportToSpawn()
    {
        PlayerController.instance.rb.MovePosition(new Vector2(spawnpoint.Item1, spawnpoint.Item2));
    }


    public void GenerateCircle(int xCenter, int yCenter, int radius)
    {
        for (int x = xCenter - radius; x <= xCenter; x++)
        {
            for (int y = yCenter - radius; y <= yCenter; y++)
            {
                // we don't have to take the square root, it's slow
                if ((x - xCenter) * (x - xCenter) + (y - yCenter) * (y - yCenter) <= radius * radius)
                {
                    //xSym = xCenter - (x - xCenter);
                    //ySym = yCenter - (y - yCenter);
                    // (x, y), (x, ySym), (xSym , y), (xSym, ySym) are in the circle
                    tiles[x, y].ID = 0;
                }
            }
        }
    }
}

public struct Tile
{
    public short ID;
    public short breakProgress;
    public Tile(short ID = 1, short breakProgress = 0)
    {
        this.ID = ID;
        this.breakProgress = breakProgress;
    }
}
public struct TileSettings
{
    public short ID;
    public Sprite sprite;
}