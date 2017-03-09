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

    public float totalHp = 10, hp = 10;
    public float banishCost = 2;
    public float age = 0;

    private List<PoisonEffect> poisons;

    void Start()
    {
        if (followPath == null) followPath = GetComponent<FollowPath>();
        followPath.speed = wave.speed;
        followPath.restartWhenDone = true;
        totalHp = wave.hp;
        hp = wave.hp;

        followPath.PathCompleted += FollowPath_PathCompleted;

        poisons = new List<PoisonEffect>();
    }

    private void Update()
    {
        age += Time.deltaTime;

        var poisonDamage = 0.0f;
        for (int q = 0; q < poisons.Count; q++)
        {
            PoisonEffect p = poisons[q];
            var deltaTime = Math.Min(Time.deltaTime, p.time);
            poisonDamage += (p.damage / p.time) * deltaTime;
            if (p.time <= 0) poisons.RemoveAt(q--);
        }
        takeDamage(poisonDamage);
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
            game.shop.money += (int)(banishCost * 2);
            game.enemiesKilled++;
            game.map.enemies.Remove(this);
            Destroy(gameObject);
        }
    }
    public void takePoison(PoisonEffect p)
    {
        poisons.Add(p);
    }

    public bool IsPoisoned
    {
        get
        {
            return poisons.Count != 0;
        }
    }
}
