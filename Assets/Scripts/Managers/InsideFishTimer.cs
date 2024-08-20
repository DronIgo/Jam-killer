using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideFishTimer : MonoBehaviour
{
    public static InsideFishTimer instance;
    public float targetTimeSec = 10f;
    public float whenToFadeSec = 15f;

    public bool underPressure = false;

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
        if (FishManager.minigameActive)
            return;

        targetTimeSec -= Time.deltaTime;

        if (targetTimeSec < 0)
        {
            Debug.Log("Timer end");
            GameManager.instance.ReturnToOcean();
        }

        if (targetTimeSec < whenToFadeSec && !underPressure)
        {
            SoundManager.instance.StartPressure();
            underPressure = true;
        }
    }

}
