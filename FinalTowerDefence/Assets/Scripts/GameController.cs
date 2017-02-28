using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MapController map;
    public WaveController waves;

    private void Start()
    {
        map.game = this;
        waves.game = this;
    }
}
