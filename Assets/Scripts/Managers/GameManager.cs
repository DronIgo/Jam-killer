using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public UIManager uiManager;
    public static int score = 10;
    public static int baitNum = 10;
    public static List<FishType> caughtFish = new List<FishType>();

    [SerializeField] Saver saver;
    [SerializeField] Saver defaultValues;
    [SerializeField] public static bool isInOcean;

    public GameObject player;

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
        LoadState();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void UpdateUIElements()
    {
        if (instance != null && instance.uiManager != null)
        {
            instance.uiManager.UpdateUI();
        }
    }

    public void SaveStateInSail()
    {
        if (isInOcean)
        {
            saver.lives = instance.player.GetComponent<Health>().currentLives;
            saver.baitNum = baitNum;
            saver.caughtFish = caughtFish;
        }
        else
        {
            saver.lives = defaultValues.lives;
            saver.baitNum = defaultValues.baitNum;
            saver.caughtFish = defaultValues.caughtFish;
            saver.score = defaultValues.score;
        }
        EditorUtility.SetDirty(saver);
    }

    public void SaveStateBetweenSails()
    {
        if (isInOcean)
        {
            saver.caughtFish = caughtFish;
        }
        else
        {
            saver.caughtFish = new List<FishType>();
        }
        EditorUtility.SetDirty(saver);
    }

    public void LoadState()
    {
        instance.player.GetComponent<Health>().currentLives = saver.lives;
        caughtFish = saver.caughtFish;
        baitNum = saver.baitNum;
        score = saver.score;
    }
}
