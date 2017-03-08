using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PoisonEffect
{
    public PoisonEffect(float damage, float time)
    {
        this.damage = damage;
        this.time = time;
    }

    public readonly float damage;
    public readonly float time;

    public PoisonEffect Clone()
    {
        return new PoisonEffect(damage, time);
    }
}
