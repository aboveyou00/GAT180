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
    public float age = 0;

    void Start()
    {
        if (followPath == null) followPath = GetComponent<FollowPath>();
        followPath.speed = wave.speed;
        followPath.restartWhenDone = true;

        followPath.PathCompleted += FollowPath_PathCompleted;
    }

    private void Update()
    {
        age += Time.deltaTime;
    }

    private void FollowPath_PathCompleted(object sender, EventArgs e)
    {
        game.shop.banish(this);
    }

    public void takeDamage(float damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            game.map.enemies.Remove(this);
            Destroy(gameObject);
        }
    }
}
