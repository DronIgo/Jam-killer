using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopStateSaver", menuName = "ScriptableObjects/ShopState", order = 6)]
public class ShopStateSaver : ScriptableObject
{
    public List<ShopChooser> alreadyBought = new List<ShopChooser>();
    public List<ShopChooser> choosen = new List<ShopChooser>();
    public int totalMoney = 0;
}
