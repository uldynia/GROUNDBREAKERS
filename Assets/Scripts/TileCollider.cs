using UnityEngine;
using Mirror;
using System;
using UnityEngine.Tilemaps;
public class TileCollider : MonoBehaviour
{
    public Action<TileBase> onCollide;
    [Server]
    void Update()
    {
        foreach (var point in GetPoints(Vector3Int.FloorToInt(transform.position)))
        {
            onCollide?.Invoke(Takama.instance.tilemap.GetTile(point));

        }
    }
    Vector3Int[] GetPoints(Vector3Int point)
    {
        return new Vector3Int[] {
        Vector3Int.FloorToInt(point + Vector3.up * 0.5f),
        Vector3Int.FloorToInt(point + Vector3.down * 0.5f),
        Vector3Int.FloorToInt(point + Vector3.left * 0.2f),
        Vector3Int.FloorToInt(point + Vector3.right * 0.2f),
            point
        };
    }
}
