using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaitDisplay : UIelement
{
    [Tooltip("The prefab to use to display the number")]
    public TextMeshProUGUI numberDisplay = null;
    [Tooltip("The maximum number of images to display before switching to just a number")]
    public int maximumNumberToDisplay = 3;


    public override void UpdateUI()
    {
        if (GameManager.instance != null)
        {
            int baitNum = GameManager.baitNum;
            SetChildImageNumber(baitNum);
        }
    }

    /// <summary>
    /// Description:
    /// Deletes and spawns images until this gameobject has as many children as the player has lives
    /// If the number of lives is over the threshold, displays a number instead
    /// Input: 
    /// int
    /// Return: 
    /// void (no return)
    /// </summary>
    /// <param name="number">The number of images that this object should have as children</param>
    private void SetChildImageNumber(int number)
    {
        numberDisplay.text = number.ToString();

    }
}
