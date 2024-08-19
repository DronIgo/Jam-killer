using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GeneratePrefab : MonoBehaviour
{
    public GameObject emptyprefab;
    public GameObject activePrefab;
    public float minX = -14.5f;
    public float minY = -14.5f;
    public float maxX = 14.5f;
    public float maxY = 14.5f;

    public GameObject gm1;
    public int amount1 = 0;

    public GameObject gm2;
    public int amount2 = 0;

    public GameObject gm3;
    public int amount3 = 0;

    public GameObject gm4;
    public int amount4 = 0;

    public GameObject gm5;
    public int amount5 = 0;

    public GameObject gm6;
    public int amount6 = 0;

    public bool generateNow = false;
    public void Generate()
    {
        if (activePrefab != null)
            DestroyImmediate(activePrefab);
        activePrefab = Instantiate(emptyprefab, new Vector3(0, 0, 0), Quaternion.identity);
        for (int i = 0; i < amount1; ++i)
        {
            var g = Instantiate(gm1, Utils.GetRandomPointInRect(minX, minY, maxX, maxY), Quaternion.identity);
            g.transform.SetParent(activePrefab.transform);
        }
        for (int i = 0; i < amount2; ++i)
        {
            var g = Instantiate(gm2, Utils.GetRandomPointInRect(minX, minY, maxX, maxY), Quaternion.identity);
            g.transform.SetParent(activePrefab.transform);
        }
        for (int i = 0; i < amount3; ++i)
        {
            var g = Instantiate(gm3, Utils.GetRandomPointInRect(minX, minY, maxX, maxY), Quaternion.identity);
            g.transform.SetParent(activePrefab.transform);
        }
        for (int i = 0; i < amount4; ++i)
        {
            var g = Instantiate(gm4, Utils.GetRandomPointInRect(minX, minY, maxX, maxY), Quaternion.identity);
            g.transform.SetParent(activePrefab.transform);
        }
        for (int i = 0; i < amount5; ++i)
        {
            var g = Instantiate(gm5, Utils.GetRandomPointInRect(minX, minY, maxX, maxY), Quaternion.identity);
            g.transform.SetParent(activePrefab.transform);
        }
        for (int i = 0; i < amount6; ++i)
        {
            var g = Instantiate(gm6, Utils.GetRandomPointInRect(minX, minY, maxX, maxY), Quaternion.identity);
            g.transform.SetParent(activePrefab.transform);
        }
    }

    private void Update()
    {
        if (generateNow)
        {
            Generate();
            generateNow = false;
        }
    }
}
