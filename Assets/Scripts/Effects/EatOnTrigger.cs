using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatOnTrigger : MonoBehaviour
{
    public bool hidePlayerThisFrame = false;
    public void GoInsideFish()
    {
        GameManager.instance.GoInsideFish();
    }

    private void Update()
    {
        if (hidePlayerThisFrame)
        {
            GameManager.instance.HidePlayer();
            hidePlayerThisFrame = false;
        }
    }
}
