using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishAI : MonoBehaviour
{
    public enum FishState { HOSTILE, SCARED, NEUTRAL };
    public IFishGoal currentGoal;
    public bool hasAGoal = false;
    public FishState currentState;
    public GameObject playerShip;
    public FishMover fishMover;
    public FishBehaviourType behaviourType;
    public float distanceToPlayer
    {
        get
        {
            return Vector3.Distance(playerShip.transform.position, transform.position);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SetGoal(new FishGoalRandomPoint(this));
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAGoal)
        {
            UpdateState();
            SetGoalAccordingToState();
        }
        if (currentGoal.CheckGoalStatus())
        {
            currentGoal.ActionOnGoalReached();
            if (!hasAGoal)
            {
                SetGoalAccordingToState();
            }
        }
        else
            currentGoal.SwimAccordingToGoal();
    }

    public void Attack()
    {
        Debug.Log("ATTACK!");
    }

    private void UpdateState()
    {

    }

    private void SetGoalAccordingToState()
    {
        if (currentState == FishState.HOSTILE && distanceToPlayer < behaviourType.distanceChase)
        {
            SetGoal(new FishGoalChase(this));
        } else
        {
            SetGoal(new FishGoalRandomPoint(this));
        }
        hasAGoal = true;
    }

    public void ResetBehaviour()
    {
        if (behaviourType == null)
            return;
        currentState = behaviourType.defaultState;
    }

    public void SetGoal(IFishGoal goal)
    {
        currentGoal = goal;
    }
}
