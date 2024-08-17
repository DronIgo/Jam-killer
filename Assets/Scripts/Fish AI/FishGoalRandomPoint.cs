using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoalRandomPoint : IFishGoal
{
    public Vector3 goalPoint;
    public Transform fishTransform;
    public float maxDistance = 25.0f;
    public float minDistance = 5.0f;
    public float goalDist = 0.1f;
    public FishGoalRandomPoint(FishAI fish) : base(fish)
    {
        fishTransform = fish.gameObject.transform;
        float angle = Random.Range(0, Mathf.PI * 2.0f);
        float dist = Random.Range(minDistance, maxDistance);
        goalPoint = new Vector3(Mathf.Sin(angle) * dist, Mathf.Cos(angle) * dist, fishTransform.position.z);
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
        fishTransform.up = goal;
        fishTransform.position += goal * fishAI.speed * Time.deltaTime;
    }
}
