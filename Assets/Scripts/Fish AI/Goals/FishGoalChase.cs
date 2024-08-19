using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoalChase : IFishGoal
{
    public Transform playerTransform;
    public FishMover fishMover;
    public float goalDist = 0.1f;
    public int attacksLeft = 1;
    public FishGoalChase(FishAI fish) : base(fish)
    {
        playerTransform = fish.playerCenter;
        fishMover = fish.fishMover;
        goalDist = fish.fishComponent.type.attackDistance;
        attacksLeft = fish.behaviourType.numOfAttacks;
    }

    public FishGoalChase(FishGoalChase prevGoal) : base(prevGoal.fishAI)
    {
        playerTransform = prevGoal.playerTransform;
        fishMover = prevGoal.fishMover;
        goalDist = prevGoal.goalDist;
        attacksLeft = prevGoal.attacksLeft;
    }
    public override void ActionOnGoalReached()
    {
        fishAI.Attack();
        attacksLeft--;
        if (attacksLeft > 0)
            fishAI.SetGoal(new FishGoalChase(this));
        fishAI.hasAGoal = false;
    }

    public override bool CheckGoalStatus()
    {
        return Vector2.Distance(playerTransform.position, fishTransform.position) < goalDist;
    }

    public override void SwimAccordingToGoal()
    {
        Vector3 goalPoint = playerTransform.position;
        Debug.DrawLine(fishTransform.position, goalPoint, Color.red, .1f, false);
        Vector3 goal = (goalPoint - fishTransform.position);
        goal.Normalize();
        fishMover.SetDirection(goal);
    }
}
