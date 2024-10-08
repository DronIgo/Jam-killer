using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Behaviour", menuName = "ScriptableObjects/FishBehaviourType", order = 2)]
public class FishBehaviourType : ScriptableObject
{
    public FishAI.FishState defaultState;
    public float distanceChase;
    public float agroDistance = 30f;
    public int numOfAttacks;

    public float defaultHungryLevel = 0.5f;
}
