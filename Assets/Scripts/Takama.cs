using Mirror;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TileRenderer))]
public class Takama : NetworkBehaviour // the main game. Izumo is the lobby.
{
    public TileSettings[] settings;
    public static Takama instance;
    [HideInInspector] public TileArray[] _tiles;
    public Tiles tiles = new();
    Tuple<int,int> spawnpoint;
    public int xsize, ysize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }
    public void Init(int _xsize, int _ysize, int missionLength) => StartCoroutine(_Init(_xsize, _ysize, missionLength));
    public IEnumerator _Init(int _xsize, int _ysize, int missionLength) // mission length is the number of paths generated.
    {
        xsize = _xsize;
        ysize = _ysize;
        instance = this;
        _tiles = new TileArray[xsize];
        Debug.Log("Starting!1");
        spawnpoint = new(xsize / 10, ysize * 9 / 10);
        for (int i = 0; i < xsize; i++)
        {
            _tiles[i] = new(ysize);
            for(int j = 0; j <  ysize; j++)
            {
                _tiles[i].tiles[j].ID = 1;
            }
        }
        Debug.Log("Starting!2");
        GenerateCircle(spawnpoint.Item1, spawnpoint.Item2, 30);
        Debug.Log("Starting!3");
        NetworkServer.Spawn(gameObject);
        Debug.Log("Starting!4");
        TeleportToSpawn();
        yield return null;
    }
    [Command]
    public void Break(short damage, int x, int y)
    {
        var tile = tiles[x, y];
        tile.breakProgress += damage;
        tiles[x, y] = tile;
    }
    [ClientRpc]
    public void TeleportToSpawn()
    {
        Debug.LogWarning("Starting!");
        PlayerController.instance.transform.position = new Vector2(spawnpoint.Item1, spawnpoint.Item2);
    }


    public void GenerateCircle(int xCenter, int yCenter, int radius)
    {
        for (int x = xCenter - radius; x <= xCenter + radius; x++)
        {
            for (int y = yCenter - radius * 2; y <= yCenter + radius; y++)
            {
                // we don't have to take the square root, it's slow
                if ((x - xCenter) * (x - xCenter) + (y - yCenter) * (y - yCenter) <= radius * radius)
                {
                    //xSym = xCenter - (x - xCenter);
                    //ySym = yCenter - (y - yCenter);
                    // (x, y), (x, ySym), (xSym , y), (xSym, ySym) are in the circle
                    var tile = tiles[x, y];
                    tile.ID = 0;
                    tiles[x,y] = tile;
                }
            }
        }
    }
}
public class Tiles
{
    public Tile this[int x, int y]
    {
        get
        {
            if (x < 0 || x > Takama.instance.xsize || y < 0 || y > Takama.instance.ysize) return new Tile();
            return Takama.instance._tiles[x].tiles[y];
        }
        set
        {
            Takama.instance._tiles[x].tiles[y] = value;
        }
    }
}
public struct TileArray
{
    public Tile[] tiles;
    public TileArray(int length)
    {
        tiles = new Tile[length];
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
[System.Serializable]
public struct TileSettings
{
    public Sprite sprite;
    public bool canCollide;
}