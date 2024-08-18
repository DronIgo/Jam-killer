using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMover : MonoBehaviour
{
    public Rigidbody2D rigidbody;
    public float maxSpeed = 2f;
    public float maxRotationSpeed = Mathf.PI;
    public float accelaration = 4f;
    public Vector3 currentSpeed = new Vector3(0, 0, 0);

    void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    public void SetDirection(Vector3 dir)
    {
        Vector3 desiredSpeed = dir.normalized * maxSpeed;
        Vector3 diff = desiredSpeed - currentSpeed;
        Vector3 speedMod = diff * accelaration * Time.deltaTime;
        if (speedMod.sqrMagnitude > diff.sqrMagnitude)
            speedMod = diff;
        currentSpeed += speedMod;
        rigidbody.velocity = currentSpeed;
        rigidbody.SetRotation(Quaternion.FromToRotation(Vector3.right, currentSpeed));
    }
    
    //private void RotateTowardsDir(Vector3 dir)
    //{
    //    currentSpeed += 
    //}
}
