using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    //private int _currentLives = 2;

    public int maxLives = 4;

    public int currentLives = 2;

    public float invincibilityTime = 3f;

    //{
    //    get
    //    {
    //        return _currentLives;
    //    }
    //    set
    //    {
    //        _currentLives = value;
    //        GameManager.UpdateUIElements();
    //    }
    //}


    // The specific game time when the health can be damaged again
    private float timeToBecomeDamagableAgain = 0;
    // Whether or not the health is invincible
    public bool isInvincible = false;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        InvincibilityCheck();
    }

    private void InvincibilityCheck()
    {
        if (timeToBecomeDamagableAgain <= Time.time)
        {
            isInvincible = false;
        }
    }

    private void CheckDeath()
    {
        if (currentLives > 0)
        {
            Debug.Log("Still Alive");
            return;
        }
        GameManager.instance.Death();
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damageAmount)
    {
        Debug.Log("Oww boat get damage!!!");

        if (currentLives <= 0 || isInvincible)
            return;

        currentLives -= damageAmount;
        
        Debug.Log("Is not invincible");
        timeToBecomeDamagableAgain = Time.time + invincibilityTime;
        isInvincible = true;

        CheckDeath();
        GameManager.UpdateUIElements();
        
        if (SoundManager.instance != null)
        {
            SoundManager.instance.OnBottleCrash();
        }
        else
        {
            Debug.LogWarning("SoundManager not exists!!!!");
        }

    }
}
