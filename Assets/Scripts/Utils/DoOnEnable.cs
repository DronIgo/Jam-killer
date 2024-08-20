using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoOnEnable : MonoBehaviour
{


    public UnityEvent doOnEnable_true;
    public UnityEvent doOnEnable_false;
    private void OnEnable()
    {
        if (GameManager.instance.canReturnHome)
        {
            if (doOnEnable_true != null)
                doOnEnable_true.Invoke();
        } else
        {
            if (doOnEnable_false != null)
                doOnEnable_false.Invoke();
        }
    }
}
