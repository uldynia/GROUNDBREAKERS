using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TileRenderer : MonoBehaviour
{
    const int xsize = 80, ysize = 30;
    Transform cam => Camera.main.transform;
    [SerializeField] SpriteRenderer p_tile;
    SpriteRenderer[,] tiles = new SpriteRenderer[xsize,ysize];
    private void Start()
    {
        for (int x = 0; x < xsize; x++)
        {
            for (int y = 0; y < ysize; y++)
            {
                var go = Instantiate(p_tile.gameObject, transform).GetComponent<SpriteRenderer>();
                go.transform.localPosition = new Vector3(x - xsize / 2, y - ysize / 2, 0);
                tiles[x,y] = go;
            }
        }
    }
    void LateUpdate()
    {
        if (Takama.instance == null) return;
        var xcenter = (int)cam.transform.position.x;
        var ycenter = (int)cam.transform.position.y;
        transform.position = new Vector3(xcenter, ycenter, 0);
        foreach(var tilesprite in tiles)
        {
            var tile = Takama.instance.tiles[(int)tilesprite.transform.position.x, (int)tilesprite.transform.position.y];
            tilesprite.sprite = Takama.instance.settings[tile.ID].sprite;
            tilesprite.GetComponent<BoxCollider2D>().enabled = Takama.instance.settings[tile.ID].canCollide;
        }
    }

}
