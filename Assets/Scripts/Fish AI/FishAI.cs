using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishAI : MonoBehaviour
{
    public enum FishState { HOSTILE, SCARED, NEUTRAL, CURIOUS, FUCKING_DONE };
    public IFishGoal currentGoal;
    public bool hasAGoal = false;
    public FishState currentState;
    public GameObject playerShip;
    public FishMover fishMover;
    public Transform fishTransform;
    public Fish fishComponent;
    public FishBehaviourType behaviourType;
    public float distanceToPlayer
    {
        get
        {
            return Vector3.Distance(playerShip.transform.position, fishTransform.position);
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        fishTransform = fishMover.gameObject.transform;
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

    public void ForceSetState(FishState state)
    {
        currentState = state;
    }

    private void UpdateState()
    {

    }

    private void SetGoalAccordingToState()
    {
        if (hasAGoal)
            return;
        if (CheckAgroCondition())
        {
            SetGoal(new FishGoalChase(this));
        } else if (CheckBaitCondition())
        {
            SetGoal(new FishGoalBait(this));
        } else 
        {
            SetGoal(new FishGoalRandomPoint(this));
        }
        hasAGoal = true;
    }

    private bool CheckAgroCondition()
    {
        return currentState == FishState.HOSTILE && distanceToPlayer < behaviourType.distanceChase;
    }

    private bool CheckBaitCondition()
    {
        if (currentState != FishState.CURIOUS)
            return false;
        if (distanceToPlayer < 10f)
            return true;
        return false;
    }

    public void StartMinigame()
    {
        fishComponent.StartMinigame();
    }

    public void ResetBehaviour()
    {
        if (currentState == FishState.FUCKING_DONE)
            return;
        if (behaviourType == null)
            return;
        currentState = behaviourType.defaultState;
    }

    public void SetGoal(IFishGoal goal)
    {
        currentGoal = goal;
        hasAGoal = true;
    }
}
