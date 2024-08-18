using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableItem : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Transform inventory;

    public void OnPointerClick(PointerEventData eventData)
    {
        var gameObject = inventory.Find("DescriptionImage").GetComponent<Image>().gameObject;
        inventory.Find("DescriptionImage").GetComponent<Image>().gameObject.SetActive(!gameObject.activeSelf);
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
