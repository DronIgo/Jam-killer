using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnTextUI : UIelement
{
    public GameObject text;
    public override void UpdateUI()
    {
        if (text != null)
            text.SetActive(GameManager.instance.canReturnHome);
    }
}
