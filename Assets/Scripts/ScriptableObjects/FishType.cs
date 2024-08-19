using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FishType", menuName = "ScriptableObjects/FishType", order = 1)]
public class FishType : ScriptableObject
{
    public enum FISH_SIZE { BIG, MEDIUM, SMALL };
    public FISH_SIZE size;

    public string fishName;
    public string description;
    public int fishCost;
    public Sprite fishIcon;
    public GameObject fishPrefab;
    public GameObject attackPrefab;

    public float attackDistance;
    public float probabilityOfAttack;
    public float probabilityOfBaiting;

    public int probability;

    public float lifeTime;

    public FishMovementType movementType;
    public FishBehaviourType behaviourType;
}
