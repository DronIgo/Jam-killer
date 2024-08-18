using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This causes the patience component to change displaying
public class PatienceTimerDisplay : UIelement
{
    public GameObject timerLine; // TODO: use it

    public Text timerText = null;
    public float currentValue = 10f;

    public void Update()
    {
        UpdateUI();
    }

    public override void UpdateUI()
    {
        // base.UpdateUI();
        // Debug.Log("UpdateUI");
        if (InsideFishTimer.instance != null)
        {
            // Debug.Log("Game manager not NULL!!");
            if (timerText != null)
            {
                // Debug.Log("timerText not NULL!!");
                timerText.text = InsideFishTimer.instance.targetTimeSec.ToString();
            }   
        }
    }
}
