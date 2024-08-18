using System.Globalization;
using System.ComponentModel;
using System.Timers;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float acceleration = 1f;
    public float deceleration = 2f;
    public Vector2 currentVelocity = Vector2.zero;

    public FishingRod fishingRod;

    // for debug
    public Vector2 move;

    private InputManager input;
    private SpriteRenderer shipRenderer;

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.instance;
        shipRenderer = GetComponent<SpriteRenderer>();
        if (fishingRod == null)
            fishingRod = GetComponentInChildren<FishingRod>();
    }

    void Update()
    {
        // get user input
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float horizontal = input.horizontalMovement;
        float vertical = input.verticalMovement;

        // move with acceleration
        move = new Vector2(horizontal, vertical).normalized;
        Vector2 accelerationVector;

        if (move != Vector2.zero)
        {
            Vector2 targetVelocity = move * maxSpeed;

            Vector2 deltaVelocity = targetVelocity - currentVelocity;
            accelerationVector = deltaVelocity.normalized * (acceleration * Time.fixedDeltaTime);

            if (accelerationVector.sqrMagnitude > deltaVelocity.sqrMagnitude)
            {
                accelerationVector = deltaVelocity;
            }
        }
        else
        {
            accelerationVector = -currentVelocity.normalized * (deceleration * Time.fixedDeltaTime);
            if(accelerationVector.sqrMagnitude > currentVelocity.sqrMagnitude){
                accelerationVector = -currentVelocity;
            }
            if (Vector2.Dot(currentVelocity, accelerationVector) > 0f)
            {
                accelerationVector = Vector2.zero;
            }
        }

        currentVelocity += accelerationVector;
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);


        // mirror if necessary
        if (horizontal < 0)
        {
            shipRenderer.flipX = true;
        }
        else if (horizontal > 0)
        {
            shipRenderer.flipX = false;
        }


        //check hook thrown
        if (input.hookThrown)
        {
            if (!FishingRod.rodActive)
            {
                fishingRod.Throw(new Vector2(0f, -0.5f));
            }
            else
                fishingRod.SetActive(false);
        }
    }
}
