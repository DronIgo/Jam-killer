using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoalRandomPoint : IFishGoal
{
    public Vector3 goalPoint;
    public Transform fishTransform;
    public float maxDistance = 20.0f;
    public float minDistance = 3.0f;
    public float goalDist = 0.1f;
    public FishMover fishMover;
    public FishGoalRandomPoint(FishAI fish) : base(fish)
    {
        fishTransform = fish.gameObject.transform;
        fishMover = fish.fishMover;
        goalPoint = Utils.GetRandomPointInArea(fishTransform.position, minDistance, maxDistance);
    }

    public override void ActionOnGoalReached()
    {
        fishAI.hasAGoal = false;
    }

    public override bool CheckGoalStatus()
    {
        return Vector3.Distance(goalPoint, fishTransform.position) < goalDist;
    }

    public override void SwimAccordingToGoal()
    {
        Debug.DrawLine(fishTransform.position, goalPoint, Color.green, .1f, false);
        Vector3 goal = (goalPoint - fishTransform.position);
        goal.Normalize();
        fishMover.SetDirection(goal);
    }
}
