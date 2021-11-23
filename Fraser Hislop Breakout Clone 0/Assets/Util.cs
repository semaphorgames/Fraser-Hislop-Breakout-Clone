using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    private static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    // Aligned s.t. 0 is up. + => CW, - => CCW
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2((degree + 90f) * Mathf.Deg2Rad);
    }
}
