using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableItem : MonoBehaviour, IPointerClickHandler
{

    public TextMeshProUGUI descriptionText;
    public ShopChooser shopItemChoser;
    public List<ShopChooser> deselectOnSelected;
    public GameObject ownText;
    public Image image;
    public bool bought;
    public bool selected;

    void Awake()
    {
        if (ownText == null)
            ownText = transform.Find("Own").gameObject; 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopManager.instance.UpdateActiveItem(this);
    }

    
    public void OnSelected()
    {
        foreach (var sc in deselectOnSelected)
        {
            ShopManager.instance.RemoveFromSelected(sc);
        }
        ShopManager.instance.SelectItem(shopItemChoser);
        selected = true;
        ShopManager.instance.UpdateChoosenAndBought();
    }

    public void UpdateUI()
    {
        ownText.SetActive(bought);
        if (bought && !selected)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
        } else
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
    }

    public bool instantSelect = false;
    public void OnBought()
    {
        ShopManager.instance.BuyItem(shopItemChoser);
        bought = true;
        if (instantSelect)
        {
            ShopManager.instance.SelectItem(shopItemChoser);
            selected = true;
        }
    }
}
