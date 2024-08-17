using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    private int _currentLives = 2;
    public int currentLives {
        get
        {
            return _currentLives;
        }
        set
        {
            _currentLives = value;
            GameManager.UpdateUIElements();
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
