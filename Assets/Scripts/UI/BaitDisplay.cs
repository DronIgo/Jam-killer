using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitDisplay : UIelement
{
    [Header("Settings")]
    [Tooltip("The prefab image to use when displaying baits remaining")]
    public GameObject baitDisplayImage = null;
    [Tooltip("The prefab to use to display the number")]
    public GameObject numberDisplay = null;
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
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        if (baitDisplayImage != null)
        {
            if (maximumNumberToDisplay >= number)
            {
                for (int i = 0; i < number; i++)
                {
                    Instantiate(baitDisplayImage, transform);
                }
            }
            else
            {
                Instantiate(baitDisplayImage, transform);
                GameObject createdNumberDisp = Instantiate(numberDisplay, transform);
                createdNumberDisp.GetComponent<Text>().text = "x " + number.ToString();
            }
        }
    }
}
