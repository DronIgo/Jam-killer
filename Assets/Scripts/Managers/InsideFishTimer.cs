using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideFishTimer : MonoBehaviour
{
    public static InsideFishTimer instance;
    public float targetTimeSec = 10f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        targetTimeSec -= Time.deltaTime;
        GameManager.UpdateUIElements();

        if (targetTimeSec < 0)
        {
            GameManager.instance.ReturnToOcean();
            // Debug.Log("Timer end");
        }
    }

}
