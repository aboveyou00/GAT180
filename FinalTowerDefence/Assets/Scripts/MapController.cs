using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameController game;

    public Vector2 offset = Vector2.zero;
    public Vector2 tileSize = new Vector2(60, 60);

    public GameObject pathTile;
    public GameObject grassTile;
    public GameObject forestTile;

    public GameObject hoverTile;
    public GameObject selectionTile;

    public Map map;

    private int selectionX, selectionY;

    void Start()
    {
        map = Map.Generate(17, 11);
        for (int q = 0; q < map.Width; q++)
        {
            for (int w = 0; w < map.Height; w++)
            {
                var point = map[q, w];
                GameObject tileType = null;
                switch (map[q, w])
                {
                case 0:
                    tileType = grassTile;
                    break;

                case 1:
                    tileType = pathTile;
                    break;

                case 2:
                    tileType = forestTile;
                    break;

                default:
                    throw new NotImplementedException();
                }
                var tile = Instantiate(tileType);
                tile.transform.position = new Vector3(offset.x + (q * tileSize.x), offset.y + (w * tileSize.y), 0);
            }
        }

        hoverTile.SetActive(false);
        selectionTile.SetActive(false);
    }

    private void Update()
    {
        var mp = Input.mousePosition;
        var x = (int)Math.Floor(mp[0] / 60);
        var y = (int)Math.Floor(mp[1] / 60);

        updateHoverTile(x, y);
    }
    private void updateHoverTile(int x, int y)
    {
        if (Input.GetMouseButtonDown(0)) updateSelection(x, y);
        var tile = map[x, y];
        if (tile == 0 && (selectionX != x || selectionY != y))
        {
            hoverTile.transform.localPosition = new Vector3(x * 60, y * 60, hoverTile.transform.localPosition.z);
            hoverTile.SetActive(true);
        }
        else
        {
            hoverTile.SetActive(false);
        }
    }
    private void updateSelection(int x, int y)
    {
        selectionX = x;
        selectionY = y;
        var tile = map[x, y];
        if (tile == 0)
        {
            selectionTile.transform.localPosition = new Vector3(x * 60, y * 60, selectionTile.transform.localPosition.z);
            selectionTile.SetActive(true);
        }
        else
        {
            selectionTile.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        if (map == null) return;
        for (int q = 0; q < map.Width; q++)
        {
            for (int w = 0; w < map.Height; w++)
            {
                var type = map[q, w];
                Gizmos.color = (type == 0 ? Color.green :
                                type == 1 ? Color.yellow :
                                type == 2 ? new Color(0, .6f, 0) :
                                            new Color(1.0f, 0, 0));
                Gizmos.DrawCube(new Vector3(q, w, 0), new Vector3(1, 1, 1));
            }
        }
    }

    public Map.Point[] GetPath()
    {
        return map.GetPath();
    }
}
