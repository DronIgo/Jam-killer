using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Saver", menuName = "ScriptableObjects/Saver", order = 5)]
public class Saver : ScriptableObject
{
    public int lives;
    public int maxLives;
    public int score;
    public int baitNum;
    public GameObject shipPrefab;
    public GameObject fishingMinigamePrefab;

    public List<FishType> smallOcean;
    public List<FishType> smallInside;
    public List<FishType> bigOcean;

    public List<FishType> caughtFish = new List<FishType>();
}