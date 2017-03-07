using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Wave
{
    public Wave(int idx, int count, float speed, int hp)
    {
        this.index = idx;
        this.count = count;
        this.speed = speed;
        this.hp = hp;
    }

    public int index;
    public int count, hp;
    public float speed;
}
