using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UIManager uiManager;
    public static int score = 10;
    public static int baitNum = 10;
    public List<FishType> caughtFish = new List<FishType>();

    public string insideFishSceneName = "InsideFish";
    public string oceanSceneName = "Ocean";
    public string shopSceneName = "Shop";

    [SerializeField] Saver saver;
    [SerializeField] Saver defaultValues;
    public bool isInOcean;

    public GameObject player;
    public CameraController cameraController;
    public Transform playerCenter;

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
        //playerCenter = player.transform.Find("center");
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

    public void AddFish(FishType type)
    {
        caughtFish.Add(type);
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
        Debug.Log(saver.lives);
        instance.player.GetComponent<Health>().currentLives = saver.lives;
        caughtFish = saver.caughtFish;
        baitNum = saver.baitNum;
        score = saver.score;
    }

    public void GoToTheShop()
    {
        SaveStateBetweenSails();

        SceneManager.LoadScene(shopSceneName);
    }

    public void GoInsideFish()
    {
        SaveStateBetweenSails();
        SaveStateInSail();

        SceneManager.LoadScene(insideFishSceneName);
    }

    public void ReturnToOcean()
    {
        SaveStateBetweenSails();
        SaveStateInSail();

        SceneManager.LoadScene(oceanSceneName);

    }
}
