using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject waveTilePrefab;
    public Vector2 waveTileSize;
    public bool horizontalWaveTiles = true;
    public float timeBeforeFirstWave = 2;
    public float timeBetweenWaves = 20;

    private int nextWaveIndex;
    private float timeUntilNextWave;
    private WaveGenerator waves;
    private List<PartialWave> partialWaves;
    private List<GameObject> waveTiles;

    void Start()
    {
        waves = new WaveGenerator();
        partialWaves = new List<PartialWave>();
        waveTiles = new List<GameObject>();

        nextWaveIndex = 0;
        timeUntilNextWave = timeBeforeFirstWave;
    }

    void Update()
    {
        timeUntilNextWave -= Time.deltaTime;
        while (timeUntilNextWave < 0)
        {
            timeUntilNextWave += timeBetweenWaves;
            var nextWave = this[nextWaveIndex++];
            partialWaves.Add(new PartialWave(nextWave));
        }

        for (int q = 0; q < partialWaves.Count; q++)
        {
            partialWaves[q].Update();
            if (partialWaves[q].IsDone) partialWaves.RemoveAt(q--);
        }
    }

    public Wave this[int idx]
    {
        get
        {
            return waves[idx];
        }
    }
}
