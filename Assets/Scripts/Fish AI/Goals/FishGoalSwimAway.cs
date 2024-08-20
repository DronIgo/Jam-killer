using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoalSwimAway : IFishGoal
{
    public Vector3 goalPoint;
    public float maxDistance = 7.0f;
    public float minDistance = 2.0f;

    public float maxY;
    public float minY;
    public bool useRestriction;

    public float goalDist = 0.1f;
    public FishMover fishMover;
    public FishGoalSwimAway(FishAI fish) : base(fish)
    {
        var moveType = fish.fishComponent.type.movementType;

        maxDistance = moveType.maxDistForGoal;
        minDistance = moveType.minDistForGoal;

        maxY = moveType.maxYForGoal;
        minY = moveType.minYForGoal;
        useRestriction = moveType.useCoordRestrictions;

        fishMover = fish.fishMover;
        goalPoint = Utils.GetRandomPointInArea(fishTransform.position, minDistance, maxDistance);
    }

    public override void ActionOnGoalReached()
    {
        fishAI.fishComponent.Destroy();
    }

    public override bool CheckGoalStatus()
    {
        return Vector2.Distance(goalPoint, fishTransform.position) < goalDist;
    }

    public override void SwimAccordingToGoal()
    {
        Debug.DrawLine(fishTransform.position, goalPoint, Color.green, .1f, false);
        Vector3 goal = (goalPoint - fishTransform.position);
        goal.Normalize();
        fishMover.SetDirection(goal);
    }
}
