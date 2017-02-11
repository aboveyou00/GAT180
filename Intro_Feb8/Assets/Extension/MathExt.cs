using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class MathExt
{
    public static float Mix(float lhs, float rhs, float amt)
    {
        return (amt * rhs) + ((1 - amt) * lhs);
    }
    public static Vector3 Mix(Vector3 lhs, Vector3 rhs, float amt)
    {
        return (amt * rhs) + ((1 - amt) * lhs);
    }
}
