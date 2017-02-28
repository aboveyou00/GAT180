using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;

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

    public void Update(WaveController waves)
    {
        if (IsDone) return;
        nextEnemyDelay -= Time.deltaTime;
        while (nextEnemyDelay <= 0)
        {
            createEnemy(waves);
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

    private void createEnemy(WaveController waves)
    {
        enemiesLeft--;
        var enemy = Object.Instantiate(waves.enemyPrefab);
        var enemyC = enemy.GetComponent<EnemyController>();
        enemyC.wave = wave;
        enemyC.followPath.map = waves.game.map;
    }
}
