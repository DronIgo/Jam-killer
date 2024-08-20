using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetRandomPointInArea(Vector3 init, float minDist, float maxDist)
    {
        float angle = Random.Range(0, Mathf.PI * 2.0f);
        float dist = Random.Range(minDist, maxDist);
        Vector3 point = new Vector3(Mathf.Sin(angle) * dist, Mathf.Cos(angle) * dist, 0) + init;
        return point;
    }

    public static Vector3 GetRandomPointInRect(float minX, float minY, float maxX, float maxY)
    {
        float x = Random.Range(minX, maxX);
        float y = Random.Range(minY, maxY);
        Vector3 point = new Vector3(x, y, 0);
        return point;
    }
}
