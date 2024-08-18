using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDoNothingGoal : IFishGoal
{
    FishMover fishMover;
    public FishDoNothingGoal(FishAI fish) : base(fish)
    {
        fishMover = fish.fishMover;
    }

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
        fishMover.Stop();
    }
}
