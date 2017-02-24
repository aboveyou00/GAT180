using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Vector2 offset = Vector2.zero;
    public Vector2 tileSize = new Vector2(60, 60);

    public GameObject pathTile;
    public GameObject grassTile;

    public Map map;

    void Start()
    {
        map = Map.Generate(17, 11);
        for (int q = 0; q < map.Width; q++)
        {
            for (int w = 0; w < map.Height; w++)
            {
                var tile = Instantiate(map.IsPointOnPath(q, w) ? pathTile : grassTile);
                tile.transform.position = new Vector3(offset.x + (q * tileSize.x), offset.y + (w * tileSize.y), 0);
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
                                            new Color(.3f, 1, 0));
                Gizmos.DrawCube(new Vector3(q, w, 0), new Vector3(1, 1, 1));
            }
        }
    }
}
