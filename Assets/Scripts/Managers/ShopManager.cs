using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public List<ClickableItem> items;
    public ScoreDisplay moneyDisplay;
    public static ShopManager instance;
    public static List<FishType> caughtFish = new List<FishType>();

    public ClickableItem curClicableItem;

    public TextMeshProUGUI descrText;
    public TextMeshProUGUI costText;

    public GameObject buyButton;
    public GameObject selectButton;

    public List<ShopChooser> bought;
    public List<ShopChooser> choosen;

    int _money;

    public int money {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            moneyDisplay.UpdateUI();
        }
    }

    [SerializeField] Saver saver;
    [SerializeField] Saver defaultValues;
    [SerializeField] Saver bonusesSaver;
    public ShopStateSaver shopStateSaver;

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
        UpdateChoosenAndBought();
        UpdateActiveItem(null);
    }

    private bool updateShip = false;
    private bool updateMinigame =false;
    private bool updateSmallFish = false;
    private bool updateBigFish = false;
    private bool updateCamera = false;

    public void UpdateBonusesState()
    {
        updateShip = false;
        updateMinigame = false;
        updateSmallFish = false;
        updateBigFish = false;
        updateCamera = false;

        bonusesSaver.baitNum = 0;   
        bonusesSaver.lives = 0;
        bonusesSaver.maxLives = 0;
        bonusesSaver.shipPrefab = defaultValues.shipPrefab;
        bonusesSaver.fishingMinigamePrefab = defaultValues.fishingMinigamePrefab;
        bonusesSaver.bigOcean = new List<FishType>();
        bonusesSaver.smallOcean = new List<FishType>();
        bonusesSaver.smallInside = new List<FishType>();
        bonusesSaver.extraCameraSize = 0;

        foreach (ShopChooser item in choosen)
        {
            if (item.extraBait)
                bonusesSaver.baitNum += item.extraBaitAmount;

            if (item.extraLives)
            {
                bonusesSaver.lives += item.extraLivesAmount;
                bonusesSaver.maxLives += item.extraLivesAmount;
            }

            if (item.updateShipPrefab)
            {
                bonusesSaver.shipPrefab = item.newShipPrefab;
                updateShip = true;
            }

            if (item.updatefishingMinigamePrefab)
            {
                bonusesSaver.fishingMinigamePrefab = item.newFishingMinigame;
                updateMinigame = true;
            }

            if (item.updateSmallFishList) {
                bonusesSaver.smallOcean = new List<FishType>(item.newSmallFishList);
                bonusesSaver.smallInside = new List<FishType>(item.newSmallFishInsideList);
                updateSmallFish = true;
            }

            if (item.updateBigFishList)
            {
                bonusesSaver.bigOcean = new List<FishType>(item.newBigFishList);
                updateBigFish = true;
            }

            if (item.extraCameraSize > 0)
            {
                bonusesSaver.extraCameraSize = item.extraCameraSize;
                updateCamera = true;
            }
        }
        #if UNITY_EDITOR
        EditorUtility.SetDirty(bonusesSaver);
        #endif
    }

    bool ContainsName(ShopChooser item, List<ShopChooser> items)
    {
        for (int i = 0; i < items.Count; ++i)
        {
            if (item.ItemName == items[i].ItemName)
                return true;
        }
        return false;
    }

    public void UpdateChoosenAndBought()
    {
        foreach (ClickableItem ci in items)
        {
            if (ContainsName(ci.shopItemChoser, bought))
                ci.bought = true;
            else
                ci.bought = false;

            if (ContainsName(ci.shopItemChoser, choosen))
                ci.selected = true;
            else
                ci.selected = false;

            ci.UpdateUI();
        }
    }

    public void GoSailing()
    {
        UpdateBonusesState();
        SaveStateInSail();
        SceneManager.LoadScene("Ocean");
    }
    
    public void BuyItem(ShopChooser item)
    {
        if (!bought.Contains(item))
        {
            bought.Add(item);
        }
        money -= item.cost;
        UpdateBonusesState();
    }

    public void SelectItem(ShopChooser item)
    {
        if (!choosen.Contains(item))
        {
            choosen.Add(item);
        }
    }

    public void RemoveFromSelected(ShopChooser item)
    {
        for (int i = 0; i < choosen.Count; ++i)
        {
            if (choosen[i].ItemName == item.ItemName)
            {
                choosen.RemoveAt(i);
                break;
            }
        }
    }

    public void BuyActiveItem()
    {
        curClicableItem.OnBought();
        UpdateChoosenAndBought();
        UpdateActiveItem(curClicableItem);
    }

    public void SelectActiveItem()
    {
        curClicableItem.OnSelected();
        UpdateChoosenAndBought();
        UpdateActiveItem(curClicableItem);
    }

    public void UpdateActiveItem(ClickableItem item)
    {
        buyButton.SetActive(false);
        selectButton.SetActive(false);
        descrText.text = "";
        costText.text = "";
        if (item == null)
        {
            return;
        }
        curClicableItem = item;
        descrText.text = item.shopItemChoser.shopItemDescription;
        costText.text = item.shopItemChoser.cost.ToString();
        if (!item.bought)
        {
            if (item.shopItemChoser.cost <= money)
                buyButton.SetActive(true);
        }
        if (item.bought && !item.selected)
        {
            selectButton.SetActive(true);
        }
    }

    public void SaveStateInSail()
    {
        saver.lives = defaultValues.lives + bonusesSaver.lives;
        saver.maxLives = defaultValues.maxLives + bonusesSaver.maxLives;
        saver.baitNum = defaultValues.baitNum + bonusesSaver.baitNum;
        saver.caughtFish = new List<FishType>();
        saver.score = 0;//defaultValues.score;
        saver.shipPrefab = defaultValues.shipPrefab;
        saver.fishingMinigamePrefab = defaultValues.fishingMinigamePrefab;

        saver.bigOcean = new List<FishType>(defaultValues.bigOcean);
        saver.smallOcean = new List<FishType>(defaultValues.smallOcean);
        saver.smallInside = new List<FishType>(defaultValues.smallInside);

        if (updateMinigame)
            saver.fishingMinigamePrefab = bonusesSaver.fishingMinigamePrefab;
        if (updateShip)
            saver.shipPrefab = bonusesSaver.shipPrefab;
        if (updateSmallFish)
        {
            List<FishType> fishTypes = new List<FishType>(defaultValues.smallOcean);
            fishTypes.AddRange(bonusesSaver.smallOcean);

            saver.smallOcean = new List<FishType>(fishTypes);
            List<FishType> fishTypesInside = new List<FishType>(defaultValues.smallInside);
            fishTypesInside.AddRange(bonusesSaver.smallInside);

            saver.smallInside = new List<FishType>(fishTypesInside);
        }
        if (updateBigFish)
        {
            List<FishType> fishTypes = new List<FishType>(defaultValues.bigOcean);
            fishTypes.AddRange(bonusesSaver.bigOcean);

            saver.bigOcean = new List<FishType>(fishTypes);
        }
        if (updateCamera)
        {
            saver.extraCameraSize = bonusesSaver.extraCameraSize;
        }
        #if UNITY_EDITOR
        EditorUtility.SetDirty(saver);
        #endif

        shopStateSaver.alreadyBought = new List<ShopChooser>(bought);
        shopStateSaver.choosen = new List<ShopChooser>(choosen);
        shopStateSaver.totalMoney = money;
        #if UNITY_EDITOR
        EditorUtility.SetDirty(shopStateSaver);
        #endif
    }

    public void LoadState()
    {
        bought = new List<ShopChooser>(shopStateSaver.alreadyBought);
        choosen = new List<ShopChooser>(shopStateSaver.choosen);
        money = shopStateSaver.totalMoney;

        caughtFish = saver.caughtFish;
        foreach (FishType ft in caughtFish)
        {
            money += ft.fishCost;
        }
    }
}
