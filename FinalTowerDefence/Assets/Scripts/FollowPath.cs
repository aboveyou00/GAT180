using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public float startDelay, speed;
    public MapController map;
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
        if (map == null || map.map == null) return;

        var path = map.GetPath();
        if (!done)
        {
            if (startDelay > 0)
            {
                startDelay -= Time.deltaTime;
                return;
            }
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
        transform.position = new Vector3(map.offset.x + (pt.x * map.tileSize.x), map.offset.y + (pt.y * map.tileSize.y), 0);
    }

    public event EventHandler PathCompleted;
}
