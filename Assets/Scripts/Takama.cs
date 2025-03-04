using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
public class Takama : NetworkBehaviour // the main game. Izumo is the lobby.
{
    public static Takama instance;
    public Tilemap tilemap;
    public List<TilePlus> tiles = new();
    [SyncVar] Vector3Int spawnpoint;
    public List<Vector3Int> points;
    [SerializeField] GameObject p_droppeditem, p_maw;
    List<Entity> entities = new();
    private void Start()
    {
        Takama.instance = this;
        tilemap = GetComponentInChildren<Tilemap>();
    }
    public void Init(int xsize, int ysize, int missionlength)
    {
        spawnpoint = new Vector3Int(xsize / 10, ysize / 10 * 8, 0);
        Vector3Int point = spawnpoint;
        List<Vector2> mawPositions = new() ;
        StartCoroutine(init());
        IEnumerator init()
        {
            SetDoor(false);
            yield return new WaitForSeconds(2f);
            for (float x = 0; x < xsize; x++)
            {
                for (float y = 0; y < ysize; y++)
                {
                    float rX = x / xsize, rY = y / ysize;

                    var t = Mathf.PerlinNoise(rX * 20, rY * 20);//generate lava
                    if (t < 0.2f)
                    {
                        SetTile(new Vector3Int((int)x, (int)y), 2);
                    }

                    t = Mathf.PerlinNoise(rX * 50, rY * 50);
                    if (t > 0.8f)
                    {
                        SetTile(new Vector3Int((int)x, (int)y), 3);
                    }
                }
                if(x%5==0)
                yield return null;
            }

            BoxFill(Vector3Int.zero, 1, 0, 0, xsize, ysize);
            GenerateCircle(spawnpoint.x, spawnpoint.y, 10);

            for (int i = 0; i < missionlength; i++) // generate main cavern
            {
                point = DrawRandomLine(point, 30, 5, 0);
                if (i % 5 == 0)
                {
                    points.Add(point);
                    GenerateCircle(point.x, point.y, 15);
                }
                if(i % 3 == 0)
                {
                 mawPositions.Add(new Vector2(point.x, point.y));
                }
            }


            EnvironmentManager.instance.lightLevel = 0f;
            EnvironmentManager.instance.background = InventoryManager.instance.items["Cave"];
            TeleportToSpawn();
            SetDoor(true);
            MissionsManager.instance.currentMission = Instantiate(MissionsManager.instance.currentMission);
            NetworkServer.Spawn(MissionsManager.instance.currentMission.gameObject);
            MissionsManager.instance.currentMission.StartMission();

            foreach(var pos in mawPositions)
            {
                var pt = Physics2D.Raycast(pos, Vector2.down).point + Vector2.up * 3;
                var maw = Instantiate(p_maw, pt, Quaternion.identity);
                NetworkServer.Spawn(maw);
            }

            foreach(var player in FindObjectsByType<PlayerController>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
            {
                entities.Add(player.GetComponent<Entity>());
            }
        }
    }
    private void Update()
    {
        if(!isServer) return;
        if (IsDead())
        {
            SetWon(false);
            EndGame();
        }
    }
    [ClientRpc]
    public void SetWon(bool won) => WinLossManager.instance.won = won;
    public bool IsDead()
    {
        foreach (Entity e in entities)
        {
            if (e.health > 0)
            {
                return false;
            }
        }
        return entities.Count > 0;
    }
    public Vector3Int DrawRandomLine(Vector3Int currentPoint, int length, int thickness, int id)
    {
        float randomAngle = Random.Range(-50f, 20f);
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

    [ClientRpc]
    public void GenerateCircle(int xCenter, int yCenter, int radius)
    {
        for (int x = xCenter - radius; x <= xCenter + radius; x++)
        {
            for (int y = yCenter - radius * 2; y <= yCenter + radius; y++)
            {
                if ((x - xCenter) * (x - xCenter) + (y - yCenter) * (y - yCenter) <= radius * radius)
                {
                    tilemap.SetTile(new Vector3Int(x, y), tiles[0]);
                }
            }
        }
    }
    bool ended = false;
    [ClientRpc] public void EndGame()
    {
        if(!ended)
            StartCoroutine(End());
        IEnumerator End()
        {
            ended = true;
            Loading.instance.SetDoor(false);
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("EndScreen");
        }
    }
    [ClientRpc] public void SetDoor(bool isOpen) => Loading.instance.SetDoor(isOpen);
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
    [Server]
    public void BreakTile(Vector3Int position)
    {
        TilePlus tile = (TilePlus)tilemap.GetTile(position);
        if (tile != null)
            if (tile.drop != null)
            {
                var dropped = Instantiate(p_droppeditem, position, Quaternion.identity).GetComponent<DroppedItem>();
                dropped.Init(tile.drop);
                NetworkServer.Spawn(dropped.gameObject);
            }
        SetTile(position, 0);
    }
    [ClientRpc]
    public void SetTile(Vector3Int position, int id)
    {
        tilemap.SetTile(position, tiles[id]);
    }
}