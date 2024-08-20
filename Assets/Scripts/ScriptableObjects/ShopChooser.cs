using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ShopItem", order = 3)]
public class ShopChooser : ScriptableObject
{
    public string shopItemDescription;
    public int cost;
    public string ItemName;

    public bool extraLives = false;
    public int extraLivesAmount = 1;

    public bool extraBait = false;
    public int extraBaitAmount = 1;

    public bool updateShipPrefab = false;
    public GameObject newShipPrefab;

    public bool updatefishingMinigamePrefab = false;
    public GameObject newFishingMinigame;
    public GameObject newBait;

    public bool updateSmallFishList = false;
    public List<FishType> newSmallFishList;
    public List<FishType> newSmallFishInsideList;

    public bool updateBigFishList = false;
    public List<FishType> newBigFishList;

    public float extraCameraSize = 0;
}
