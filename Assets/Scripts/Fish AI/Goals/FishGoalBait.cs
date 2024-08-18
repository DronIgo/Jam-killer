using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishGoalBait : IFishGoal
{
    public Transform playerBaitTransform;
    public FishMover fishMover;
    public float goalDist = 0.1f;
    public float timeSinceArrival = 0;
    public float timeToStartGame = 0.8f;
    public FishGoalBait(FishAI fish) : base(fish)
    {
        playerBaitTransform = fish.playerShip.GetComponent<ShipController>().fishingRod.bait;
        fishMover = fish.fishMover;
    }

    public override void ActionOnGoalReached()
    {
        fishAI.hasAGoal = false;
        if (!lostBait)
            fishAI.StartMinigame();
    }

    bool lostBait = false;

    public override bool CheckGoalStatus()
    {
        if (!FishingRod.rodActive)
        {
            lostBait = true;
            return true;
        }
        if (Vector2.Distance(playerBaitTransform.position, fishTransform.position) < goalDist)
            timeSinceArrival += Time.deltaTime;
        else
            timeSinceArrival = 0.0f;
        if (timeSinceArrival >= timeToStartGame)
            return true;
        return false;
    }

    public override void SwimAccordingToGoal()
    {
        Vector3 goalPoint = playerBaitTransform.position;
        Debug.DrawLine(fishTransform.position, goalPoint, Color.black, .1f, false);
        Vector3 goal = (goalPoint - fishTransform.position);
        goal.Normalize();
        fishMover.SetDirection(goal);
    }
}
