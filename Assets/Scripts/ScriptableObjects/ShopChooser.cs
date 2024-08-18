using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopChooser", menuName = "ScriptableObjects/ShopChooser", order = 3)]
public class ShopChooser : ScriptableObject
{
    public Sprite shopItemImage;
    public string shopItemDescription;
}
