using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IFishGoal
{
    public FishAI fishAI;
    public Transform fishTransform;
    public IFishGoal(FishAI fish)
    {
        fishTransform = fish.fishTransform;
        fishAI = fish;
    }
    public abstract bool CheckGoalStatus();
    public abstract void SwimAccordingToGoal();
    public abstract void ActionOnGoalReached();
}
