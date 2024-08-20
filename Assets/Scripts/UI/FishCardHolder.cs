using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FishCardHolder : UIelement
{
    public GameObject fishCardPrefab;
    public List<GameObject> fishCards;

    private void AddFishCard(FishType fishType)
    {
        GameObject newCard = Instantiate(fishCardPrefab);
        newCard.transform.SetParent(transform);
        newCard.transform.Find("CardNameText").GetComponent<TextMeshProUGUI>().text = fishType.fishName;
        newCard.transform.Find("CardCostText").GetComponent<TextMeshProUGUI>().text = fishType.fishCost.ToString();
        newCard.transform.Find("CardIcon").GetComponent<Image>().sprite = fishType.fishIcon;
        newCard.transform.localScale = new Vector3(1, 1, 1);
        Image cardImage = newCard.GetComponent<Image>();
        if (fishType.fishCost < 4)
            cardImage.color = Color.grey;
        else if (fishType.fishCost < 8)
            cardImage.color = Color.green;
        else if (fishType.fishCost < 15)
            cardImage.color = Color.yellow;
        else 
            cardImage.color = Color.cyan;
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
