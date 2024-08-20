using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    public float maskSize = 220.0f;
    [SerializeField] RectTransform topPivot;
    [SerializeField] RectTransform bottomPivot;

    [SerializeField] RectTransform fish;

    float fishPosition;
    float fishDestination;

    float fishTimer;
    [SerializeField] float timerMultiplicator = 3f;

    float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    [SerializeField] RectTransform hook;
    float hookPosition;
    [SerializeField] float hookSize = 0.1f;
    [SerializeField] float progressSpeed = 0.5f;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.005f;
    [SerializeField] float progressDegradationSpeed = 0.01f;

    //[SerializeField] SpriteRenderer hookSpriteRenderer;

    [SerializeField] RectMask2D progressBarMask;
    [SerializeField] RectMask2D hookMask;

    //[SerializeField] Transform progressBarContainer;

    bool pause = false;
    public bool win { get; private set; }
    public bool lose { get; private set; }

    [SerializeField] float defaultFailTime = 20f;
    float failTime = 10f;

    private void Start()
    {
        Resize();
    }

    public void SetParamsFromFish(Fish fish)
    {
        timerMultiplicator = fish.type.attackDistance;
        progressSpeed = fish.type.probabilityOfAttack;
    }
        
    private void Resize()
    {
        hookMask.padding = new Vector4(hookMask.padding.x, maskSize * (1 - hookSize) / 2, hookMask.padding.z , maskSize * (1 - hookSize) / 2);
    }

    public void InitiateGame()
    {
        pause = false;
        failTime = defaultFailTime;
        hookProgress = 0.0f;
        fishPosition = 0.5f;
        hookPosition = 0.5f;
        hookPullVelocity = 0.0f;
        win = false;
        lose = false;
    }

    private void Update()
    {
        if (pause) return;
        UpdateFishPosition();
        UpdateHookPositionAndSpeed();
        UpdateModel();
        ProgressCheck();
    }

    private void UpdateModel()
    {
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
        progressBarMask.padding = new Vector4(progressBarMask.padding.x, progressBarMask.padding.y, progressBarMask.padding.z, maskSize * (1 - hookProgress));
    }

    private void ProgressCheck()
    {
        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if (min < fishPosition && max > fishPosition)
        {
            hookProgress += progressSpeed * Time.deltaTime;
        }
        else
        {
            hookProgress -= progressDegradationSpeed * Time.deltaTime;

            failTime -= Time.deltaTime;
            if (failTime < 0f)
            {
                Lose();
            }
            if (failTime < defaultFailTime - 1f && hookProgress == 0)
            {
                Lose();
            } 
        }

        if (hookProgress >= 1f)
        {
            Win();
        }

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);
    }

    private void Lose()
    {
        pause = true;
        lose = true;
        Debug.Log("fuck off eat shit and die (lose)");
    }

    private void Win()
    {
        pause = true;
        win = true;
        Debug.Log("fuck off eat shit and die (win)");
    }

    private void UpdateHookPositionAndSpeed()
    {
        if (InputManager.instance.hookUp)
        {
            hookPullVelocity += hookPullPower * Time.deltaTime;
        }
        else
        {
            hookPullVelocity -= hookGravityPower * Time.deltaTime;
        }
        hookPosition += hookPullVelocity;

        if (hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f)
        {
            hookPullVelocity = 0f;
        }
        if (hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f)
        {
            hookPullVelocity = 0f;
        }

        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);
    }

    private void UpdateFishPosition()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0f)
        {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;
            fishDestination = UnityEngine.Random.value * 1f;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
    }
}
