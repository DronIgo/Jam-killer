using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public static ShopManager instance;
    public static List<FishType> caughtFish = new List<FishType>();

    [SerializeField] Saver saver;
    [SerializeField] Saver defaultValues;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void UpdateDefaultState()
    {

    }

    public void GoSailing()
    {
        SaveStateInSail();
    }
    
    public void SaveStateInSail()
    {
        saver.lives = defaultValues.lives;
        saver.baitNum = defaultValues.baitNum;
        saver.caughtFish = defaultValues.caughtFish;
        saver.score = defaultValues.score;
        EditorUtility.SetDirty(saver);
    }

    public void LoadState()
    {
        caughtFish = saver.caughtFish;
    }
}
