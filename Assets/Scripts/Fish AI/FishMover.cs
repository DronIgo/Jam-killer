using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMover : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float maxSpeed = 2f;
    public float maxRotationSpeed = Mathf.PI;
    public float accelaration = 4f;
    public float stopDistance; 
    public Vector3 currentSpeed = new Vector3(0, 0, 0);

    void Awake()
    {   
        currentSpeed = new Vector3(0, 0, 0);
        //rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.position += currentSpeed * Time.deltaTime;
        if (currentSpeed.sqrMagnitude > 0)
        {
            transform.up = currentSpeed.normalized;
            //transform.forward = Vector3.forward;
        }
    }

    /// <summary>
    /// For unstationary targets
    /// </summary>
    /// <param name="dir"></param>
    public void SetDirection(Vector3 dir)
    {
        dir.z = 0;
        Vector3 desiredSpeed;
        desiredSpeed = dir.normalized * maxSpeed;
        Vector3 diff = desiredSpeed - currentSpeed;
        Vector3 speedMod = diff * accelaration * Time.deltaTime;
        if (speedMod.sqrMagnitude > diff.sqrMagnitude)
            speedMod = diff;
        currentSpeed += speedMod;
        currentSpeed.z = 0;
    }

    /// <summary>
    /// For stationary targets
    /// </summary>
    /// <param name="goal"></param>
    public void SetGoal(Vector3 goal)
    {
        
    }

    public void Stop()
    {
        currentSpeed = new Vector3(0, 0, 0);
    }

    //private void RotateTowardsDir(Vector3 dir)
    //{
    //    currentSpeed += 
    //}
}
