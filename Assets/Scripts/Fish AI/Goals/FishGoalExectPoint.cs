using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoalExectPoint : IFishGoal
{
    public Vector3 goalPoint;
    public float maxDistance = 20.0f;
    public float minDistance = 3.0f;

    public float maxY;
    public float minY;
    public bool useRestriction;

    public float goalDist = 0.1f;
    public FishMover fishMover;
    public FishGoalExectPoint(FishAI fish, Vector3 point) : base(fish)
    {
        fishMover = fish.fishMover;
        goalPoint = point;
    }

    public override void ActionOnGoalReached()
    {
        
        fishAI.hasAGoal = false;
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
