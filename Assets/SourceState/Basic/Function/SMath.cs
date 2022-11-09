using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SMath
{
    public static Vector2 GetRandomValueOnCircle(float radius)
    {
        float thet=Random.value*2*Mathf.PI;
        float rx=radius*Mathf.Cos(thet);
        float ry=radius*Mathf.Sin(thet);

        return new Vector2(rx, ry);
    }
}
