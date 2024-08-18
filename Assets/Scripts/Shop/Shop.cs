using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Shop : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform shopItemTemplate;
    [SerializeField] private Sprite exampleSprite;//todo example

    private void Awake()
    {
        shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton(exampleSprite, "item 1", 10, 0);
        CreateItemButton(exampleSprite, "item 2", 20, 1);
    }

    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex)
    {

        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        Transform shopItemRectTransform = shopItemTransform.GetComponent<Transform>();

        float shopItemHeight = 30f;
        shopItemRectTransform.position = new Vector3(0, 50 * positionIndex, 0);
        
        shopItemTransform.Find("shopItemText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("shopItemCost").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        //shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;

        shopItemTransform.gameObject.SetActive(true);
    }
}
