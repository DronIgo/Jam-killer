using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementType", menuName = "ScriptableObjects/FishMovementType", order = 6)]
public class FishMovementType : ScriptableObject
{
    public float minDistForGoal = 3f;
    public float maxDistForGoal = 8f;

    public float minDistForSwimAwayGoal = 2f;
    public float maxSwimForSwimAwayGoal = 5f;

    public bool useCoordRestrictions = false;

    public float maxYForGoal = 14f;
    public float minYForGoal = -14f;
}
