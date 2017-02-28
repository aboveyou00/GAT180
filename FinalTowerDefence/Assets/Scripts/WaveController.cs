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
    public float preloadWaveTileCount = 10;
    public GameController game;

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
            Destroy(waveTiles[nextWaveIndex]);
            var nextWave = this[nextWaveIndex++];
            var timeBetweenEnemies = (((nextWaveIndex + 5) / 50.0f) * timeUntilNextWave) / nextWave.count;
            partialWaves.Add(new PartialWave(nextWave, 0, timeBetweenEnemies));
        }

        if (!horizontalWaveTiles)
        {
            throw new NotImplementedException();
        }
        else
        {
            while (waveTiles.Count < nextWaveIndex + preloadWaveTileCount)
            {
                var waveTile = Instantiate(waveTilePrefab);
                waveTile.transform.parent = this.transform;
                waveTile.transform.localPosition = new Vector3(160 * waveTiles.Count, 0, 0);
                var wtController = waveTile.GetComponent<WaveTileController>();
                wtController.wave = waves[waveTiles.Count];
                waveTiles.Add(waveTile);
            }

            this.transform.localPosition = new Vector3(((-nextWaveIndex + timeUntilNextWave / timeBetweenWaves) * waveTileSize.x), 0, 0);
        }

        for (int q = 0; q < partialWaves.Count; q++)
        {
            partialWaves[q].Update(this);
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
