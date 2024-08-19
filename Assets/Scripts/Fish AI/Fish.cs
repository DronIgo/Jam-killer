using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public FishType type;
    public FishManager fishManager;
    private void Awake()
    {
        StartCoroutine("Appear");
    }

    private IEnumerator Appear()
    {
        
        yield return new WaitForEndOfFrame();
        //hookThrown = false;
    }

    public void Destroy()
    {
        fishManager.DeleteFish(gameObject);
    }

    public void OnDestroy()
    {
        //Instantiate(type.swimAwayPrefab, transform.position, transform.rotation);
    }

    public void StartMinigame()
    {
        fishManager.InitiateMinigame(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
