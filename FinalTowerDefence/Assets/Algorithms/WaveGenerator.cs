using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class WaveGenerator
{
    public WaveGenerator()
    {
        waves = new Dictionary<int, Wave>();
    }

    private Dictionary<int, Wave> waves;
    public Wave this[int idx]
    {
        get
        {
            if (idx < 0) throw new ArgumentOutOfRangeException("idx");
            if (!waves.ContainsKey(idx)) waves[idx] = makeWave(idx);
            return waves[idx];
        }
    }

    private Wave makeWave(int idx)
    {
        var count = (int)Math.Pow(5 * ((idx + 5) / 5.0f), .8);
        var speed = 3 + (idx / 10.0f);
        return new Wave(idx, count, speed);
    }
}
