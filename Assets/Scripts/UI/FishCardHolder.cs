using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FishCardHolder : UIelement
{
    public GameObject fishCardPrefab;
    public List<GameObject> fishCards;

    public Color cheapestColor = Color.blue;
    public Color cheapColor = Color.blue;
    public Color mediumColor = Color.blue;
    public Color expensiveColor = Color.blue;
    public Color legendaryColor = Color.blue;

   
    public int cheapestCost = 4;
    public int cheapCost = 10;
    public int mediumCost = 20;
    public int expensiveCost = 30;
    public int legendaryCost = 40;

    private void AddFishCard(FishType fishType)
    {
        GameObject newCard = Instantiate(fishCardPrefab);
        newCard.transform.SetParent(transform);
        newCard.transform.Find("CardNameText").GetComponent<TextMeshProUGUI>().text = fishType.fishName;
        newCard.transform.Find("CardCostText").GetComponent<TextMeshProUGUI>().text = fishType.fishCost.ToString();
        newCard.transform.Find("CardIcon").GetComponent<Image>().sprite = fishType.fishIcon;
        newCard.transform.localScale = new Vector3(1, 1, 1);
        Image cardImage = newCard.GetComponent<Image>();

        if (fishType.fishCost < cheapestCost)
            cardImage.color = cheapestColor;
        else if (fishType.fishCost < cheapCost)
            cardImage.color = cheapColor;
        else if (fishType.fishCost < mediumCost)
            cardImage.color = mediumColor;
        else if (fishType.fishCost < expensiveCost)
            cardImage.color = expensiveColor;
        else 
            cardImage.color = legendaryColor;
        
        fishCards.Add(newCard);
    }

    public override void UpdateUI()
    {
        List<FishType> caughtFish = GameManager.instance.caughtFish;
        if (caughtFish.Count == fishCards.Count)
            return;
        for (int i = fishCards.Count; i < caughtFish.Count; ++i)
        {
            AddFishCard(caughtFish[i]);
        }
    }
}
