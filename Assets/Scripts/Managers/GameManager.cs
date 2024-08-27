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

    public bool _canReturnHome = false;
    public bool canReturnHome
    {
        get
        {
            return _canReturnHome;
        }
        set
        {
            _canReturnHome = value;
            uiManager.UpdateUI();
        }
    }

    public string insideFishSceneName = "InsideFish";
    public string oceanSceneName = "Ocean";
    public string shopSceneName = "Shop";

    [SerializeField] Saver saver;
    [SerializeField] Saver defaultValues;
    public bool isInOcean;

    public GameObject player;
    public CameraController cameraController;
    public Transform playerCenter;

    public bool gameStarted = false;
    public bool fishManagerStarted = false;

    public FishManager fishManager;

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
        SetPause(false);
        FishManager.minigameActive = false;
        LoadState();
        StartCoroutine("StartGame");
        //Debug.Log("Awake");
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(0.5f);
        gameStarted = true;
        yield return new WaitForSeconds(0.5f);
        fishManagerStarted = true;
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
        saver.lives = instance.player.GetComponent<Health>().currentLives;
        saver.baitNum = baitNum;
        saver.caughtFish = new List<FishType>(caughtFish);

        #if UNITY_EDITOR
        EditorUtility.SetDirty(saver);
        #endif
    }

    public void SaveStateBetweenSails()
    {
        saver.caughtFish = new List<FishType>(caughtFish);

        #if UNITY_EDITOR
        EditorUtility.SetDirty(saver);
        #endif
    }

    public void LoadState()
    {
        Debug.Log(saver.lives);

        player = Instantiate(saver.shipPrefab, Vector3.zero, Quaternion.identity);
        player.GetComponent<ShipController>().cameraSize += saver.extraCameraSize;
        playerCenter = player.transform.Find("Center");
        var playerHealth = instance.player.GetComponent<Health>();
        playerHealth.currentLives = saver.lives;
        playerHealth.maxLives = saver.maxLives;
        caughtFish = new List<FishType>(saver.caughtFish);
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

        fishManager.UpdateFishLists(avialableFish);


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
        FishManager.minigameActive = false;
        SetPause(true);
    }

    public void HidePlayer()
    {
        player.SetActive(false);
    }
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
