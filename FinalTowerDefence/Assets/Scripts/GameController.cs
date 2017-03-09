using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MapController map;
    public WaveController waves;
    public ShopController shop;

    public float playSpeed = 1;

    public int enemiesKilled = 0;
    public int enemiesBanished = 0;
    public int moneysSpent = 0;
    public float timeSurvived = 0;

    private void Start()
    {
        map.game = this;
        waves.game = this;
        shop.game = this;
    }

    private void Update()
    {
        timeSurvived += Time.deltaTime;
    }
}
