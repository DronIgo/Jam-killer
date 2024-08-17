using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoalChase : IFishGoal
{
    public Transform playerTransform;
    public Transform fishTransform;
    public float goalDist = 0.1f;
    public FishGoalChase(FishAI fish) : base(fish)
    {
        playerTransform = fish.playerShip.transform;
        fishTransform = fish.gameObject.transform;
}
    public override void ActionOnGoalReached()
    {
        fishAI.Attack();
        fishAI.hasAGoal = false;
    }

    public override bool CheckGoalStatus()
    {
        return Vector3.Distance(playerTransform.position, fishTransform.position) < goalDist;
    }

    public override void SwimAccordingToGoal()
    {
        Vector3 goalPoint = playerTransform.position;
        Debug.DrawLine(fishTransform.position, goalPoint, Color.red, .1f, false);
        Vector3 goal = (goalPoint - fishTransform.position);
        goal.Normalize();
        fishTransform.up = goal;
        fishTransform.position += goal * fishAI.speed * Time.deltaTime;
    }
}
