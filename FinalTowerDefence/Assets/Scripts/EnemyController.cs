using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FollowPath))]
public class EnemyController : MonoBehaviour
{
    public GameController game;
    
    public FollowPath followPath;
    public Wave wave;

    public float hp = 10;
    public float banishCost = 2;

    void Start()
    {
        if (followPath == null) followPath = GetComponent<FollowPath>();
        followPath.speed = wave.speed;
        followPath.restartWhenDone = true;

        followPath.PathCompleted += FollowPath_PathCompleted;
    }
    
    private void FollowPath_PathCompleted(object sender, EventArgs e)
    {
        game.shop.banish(this);
    }
}
