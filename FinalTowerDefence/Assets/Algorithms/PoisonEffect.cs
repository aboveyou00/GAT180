using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public struct PoisonEffect
{
    public PoisonEffect(float damage, float time)
    {
        this.damage = damage;
        this.time = time;
        startTime = Time.time;
    }

    public readonly float damage;
    public readonly float time;
    public readonly float startTime;

    public PoisonEffect Restart()
    {
        return new PoisonEffect(damage, time);
    }
}
