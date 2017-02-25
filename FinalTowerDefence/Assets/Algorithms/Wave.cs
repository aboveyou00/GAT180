using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Wave
{
    public Wave(int idx, int count, float speed)
    {
        this.index = idx;
        this.count = count;
        this.speed = speed;
    }

    public int index;
    public int count;
    public float speed;
}
