using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PartialWave
{
    public PartialWave(Wave wave, float startDelay = 0, float enemyDelay = 2)
    {
        this.wave = wave;
        this.nextEnemyDelay = startDelay;
        this.enemyDelay = enemyDelay;
        this.enemiesLeft = wave.count;
    }

    public Wave wave;
    public float enemyDelay;
    public int enemiesLeft;
    public float nextEnemyDelay;

    public void Update()
    {
        if (IsDone) return;
        nextEnemyDelay -= Time.deltaTime;
        while (nextEnemyDelay <= 0)
        {
            createEnemy();
            if (IsDone) return;
            nextEnemyDelay += enemyDelay;
        }
    }
    public bool IsDone
    {
        get
        {
            return enemiesLeft == 0;
        }
    }

    private void createEnemy()
    {
        enemiesLeft--;

    }
}
