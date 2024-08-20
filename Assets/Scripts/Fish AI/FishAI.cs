using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishAI : MonoBehaviour
{
    public enum FishState { HOSTILE, SCARED, NEUTRAL, CURIOUS, FUCKING_DONE };
    float hungry = 0.5f;
    public IFishGoal currentGoal;
    public bool hasAGoal = false;
    public FishState currentState;
    public GameObject playerShip;
    public Transform playerCenter;
    public FishMover fishMover;
    public Transform fishTransform;
    public Fish fishComponent;
    public FishBehaviourType behaviourType;
    public GameObject attackEffect;
    public static float fishArgoRadiusModifier = 0.5f;
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
        hasAGoal = false;
        //SetGoal(new FishGoalRandomPoint(this));
        playerShip = GameManager.instance.player;
        playerCenter = GameManager.instance.playerCenter;
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
        TemporaryEffect attack = Instantiate(attackEffect, fishTransform.position, Quaternion.identity).GetComponent<TemporaryEffect>();
        attack.parent = this.gameObject.transform.parent.gameObject;
        transform.parent.gameObject.SetActive(false);
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
        } else if (CheckSwimAwayCondition())
        {
            SetGoal(new FishGoalSwimAway(this));
        } else
        {
            SetGoal(new FishGoalRandomPoint(this));
        }
        hasAGoal = true;
    }

    private bool CheckSwimAwayCondition()
    {
        return currentState == FishState.FUCKING_DONE;
    }

    private bool CheckAgroCondition()
    {
        if (!(currentState == FishState.HOSTILE))
            return false;
        if (!(distanceToPlayer < fishArgoRadiusModifier * behaviourType.agroDistance))
            return false;
        if (hungry < Random.Range(0.0f, 1.0f))
            return false;
        return true;
    }

    private bool CheckBaitCondition()
    {
        if (currentState != FishState.CURIOUS)
            return false;
        if (distanceToPlayer >= 5f)
            return false;
        if (hungry < Random.Range(0.0f, 1.0f)) ;
        if (!FishingRod.rodActive)
            return false;
        return true;
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
        hungry = behaviourType.defaultHungryLevel;
    }

    public void SetGoal(IFishGoal goal)
    {
        currentGoal = goal;
        hasAGoal = true;
    }
}
