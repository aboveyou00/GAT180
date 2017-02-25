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
        return new Wave(idx, (int)Math.Pow(5 + idx, .8), 3 + (idx / 10.0f));
    }
}
