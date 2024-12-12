using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Takama : NetworkBehaviour // the main game. Izumo is the lobby.
{
    public static Takama instance;
    Tilemap tilemap;
    [SerializeField] List<TileBase> tiles = new();
    [SyncVar] Vector3Int spawnpoint;
    [SerializeField] GameObject p_shadow;
    private void Start()
    {
        Takama.instance = this;
        tilemap = GetComponentInChildren<Tilemap>();
    }
    public void Init(int xsize, int ysize, int missionlength)
    {
        spawnpoint = new Vector3Int(xsize / 10, ysize / 10 * 9, 0);
        Vector3Int point = spawnpoint;
        StartCoroutine(init());
        IEnumerator init()
        {
            BoxFill(Vector3Int.zero, 1, 0, 0, xsize, ysize);
            GenerateCircle(spawnpoint.x, spawnpoint.y, 10);
            for (int i = 0; i < missionlength; i++)
            {
                point = DrawRandomLine(point, 30, 5, 0);
                if(i % 5 == 0)
                {
                    GenerateCircle(point.x, point.y, 15);
                    NetworkServer.Spawn(Instantiate(p_shadow, new Vector3(point.x, point.y), Quaternion.identity)); 
                }
            }
            yield return new WaitForSeconds(1);
            TeleportToSpawn();
        }
    }
    public Vector3Int DrawRandomLine(Vector3Int currentPoint, int length, int thickness, int id)
    {
        float randomAngle = Random.Range(-60f, 0f);
        //if (Random.Range(0, 2) == 1) randomAngle -= 60;
        float radians = randomAngle * Mathf.Deg2Rad;

        // Calculate the direction vector
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));

        // Calculate the end point using the direction vector and length
        Vector3Int endPoint = currentPoint + new Vector3Int(Mathf.RoundToInt(direction.x * length), Mathf.RoundToInt(direction.y * length), 0);

        // Call LineFill to draw a line segment to the new point
        LineFill(id, currentPoint, endPoint, thickness);

        return endPoint;
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