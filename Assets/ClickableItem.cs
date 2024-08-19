using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableItem : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Transform inventory;
    [SerializeField] private ShopChooser shopItemChoser;

    public void OnPointerClick(PointerEventData eventData)
    {
        var textObject = inventory.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        textObject.text = shopItemChoser.shopItemDescription;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
