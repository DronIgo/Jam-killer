using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FishingMinigame : MonoBehaviour
{
    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;

    [SerializeField] Transform fish;

    float fishPosition;
    float fishDestination;

    float fishTimer;
    [SerializeField] float timerMultiplicator = 3f;

    float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    [SerializeField] Transform hook;
    float hookPosition;
    [SerializeField] float hookSize = 0.1f;
    [SerializeField] float hookPower = 0.5f;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.005f;
    [SerializeField] float hookProgressDegradationPower = 0.01f;

    [SerializeField] SpriteRenderer hookSpriteRenderer;

    [SerializeField] Transform progressBarContainer;

    bool pause = false;

    [SerializeField] float failTime = 10f;

    private void Start()
    {
        Resize();
    }

    private void Resize()
    {
        Vector3 ls = hook.localScale;
        float distance = Vector3.Distance(topPivot.position, bottomPivot.position);
        ls.y = (distance * hookSize);
        hook.localScale = ls;
    }

    private void Update()
    {
        if (pause) return;
        Fish();
        Hook();
        ProgressCheck();
    }

    private void ProgressCheck()
    {
        Vector3 ls = progressBarContainer.localScale;
        ls.y = hookProgress;
        progressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if (min < fishPosition && max > fishPosition)
        {
            hookProgress += hookPower * Time.deltaTime;
        }
        else
        {
            hookProgress -= hookProgressDegradationPower * Time.deltaTime;

            failTime -= Time.deltaTime;
            if (failTime < 0f)
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
        Debug.Log("fuck off eat shit and die (lose)");
    }

    private void Win()
    {
        pause = true;
        Debug.Log("fuck off eat shit and die (win)");
    }

    private void Hook()
    {
        if (Input.GetMouseButton(0))//todo use input manager
        {
            hookPullVelocity += hookPullPower * Time.deltaTime;
        }
        hookPullVelocity -= hookGravityPower * Time.deltaTime;
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

        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }

    private void Fish()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0f)
        {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;
            fishDestination = UnityEngine.Random.value * 1f;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }
}
