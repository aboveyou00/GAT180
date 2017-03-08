using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MapController map;
    public WaveController waves;
    public ShopController shop;

    public float playSpeed = 1;

    private void Start()
    {
        map.game = this;
        waves.game = this;
        shop.game = this;
    }
}
