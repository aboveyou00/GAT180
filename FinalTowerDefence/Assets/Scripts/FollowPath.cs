using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public float speed;
    public GameController game;
    public bool restartWhenDone = false;

    private int currentIdx = 0;
    private float currentProgress = 0;
    private bool done = false;
    
    private Vector2 ptv(Map.Point pt)
    {
        return new Vector2(pt.x, pt.y);
    }
    private Vector2 mix(Map.Point one, Map.Point two, float alpha)
    {
        return (ptv(two) * alpha) + (ptv(one) * (1 - alpha));
    }
    void Update()
    {
        if (game == null || game.map == null) return;

        var path = game.map.GetPath();
        if (!done)
        {
            currentProgress += Time.deltaTime * speed;
            while (!done && currentProgress >= 1)
            {
                currentIdx++;
                currentProgress -= 1;
                if (currentIdx == path.Length - 1)
                {
                    currentProgress = 0;
                    done = true;
                }
            }
        }
        Vector2 pt;
        if (currentProgress == 0) pt = ptv(path[currentIdx]);
        else pt = mix(path[currentIdx], path[currentIdx + 1], currentProgress);
        transform.position = new Vector3(game.offset.x + (pt.x * game.tileSize.x), game.offset.y + (pt.y * game.tileSize.y), 0);
    }

    public event EventHandler PathCompleted;
}
