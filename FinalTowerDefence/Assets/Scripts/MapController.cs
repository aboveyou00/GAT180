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
    public GameObject castleTile;
    
    public Map map;
    
    void Start()
    {
        map = Map.Generate(17, 11);
        for (int q = 0; q < map.Width; q++)
        {
            for (int w = 0; w < map.Height; w++)
            {
                var point = map[q, w];
                GameObject tileType = null;
                float tileDepth = 0;
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

                case 3:
                    tileType = castleTile;
                    tileDepth = -.5f;
                    break;

                default:
                    throw new NotImplementedException();
                }
                var tile = Instantiate(tileType);
                tile.transform.position = new Vector3(offset.x + (q * tileSize.x), offset.y + (w * tileSize.y), tileDepth);
            }
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
