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
        SetPause(false);
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
        score += type.fishCost;
        UpdateUIElements();
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
        var playerHealth = instance.player.GetComponent<Health>();
        playerHealth.currentLives = saver.lives;
        playerHealth.maxLives = saver.maxLives;
        caughtFish = saver.caughtFish;
        score = 0;
        foreach (var ft in caughtFish)
            score += ft.fishCost;
        baitNum = saver.baitNum;
        List<FishType> avialableFish;
        if (isInOcean)
        {
            avialableFish = new List<FishType>(saver.bigOcean);
            avialableFish.AddRange(saver.smallOcean);
        } else
        {
            avialableFish = new List<FishType>(saver.smallInside);
        }

        FishManager.instance.UpdateFishLists(avialableFish);

        UpdateUIElements();
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

    public void Death()
    {
        caughtFish.Clear();
        UpdateUIElements();
        uiManager.GoToPageByName("DeathPage");
        uiManager.allowPause = false;
        SetPause(true);
    }

    private bool curPause;
    public void SetPause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1;
        }
    }
}
