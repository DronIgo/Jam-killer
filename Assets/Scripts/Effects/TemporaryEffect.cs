using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryEffect : MonoBehaviour
{
    public GameObject parent;
    public bool setParentActive;
    public bool destroyOnThisFrame = false;

    public void Update()
    {
        if (destroyOnThisFrame)
            Destroy();
    }

    public void Destroy()
    {
        if (setParentActive)
            parent.SetActive(true);
        Destroy(this.gameObject);
    }
}
