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
}
