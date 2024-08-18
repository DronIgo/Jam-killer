using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDoNothingGoal : IFishGoal
{
    public FishDoNothingGoal(FishAI fish) : base(fish)
    { }

    public override void ActionOnGoalReached()
    {
        return;
    }

    public override bool CheckGoalStatus()
    {
        return false;
    }

    public override void SwimAccordingToGoal()
    {
        return;
    }
}
