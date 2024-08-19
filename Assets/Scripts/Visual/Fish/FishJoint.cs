using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishJoint : MonoBehaviour
{
    public Transform Parent;
    public Vector3 rotAroundPoint;
    public Vector2 currentLookDir;
    public float rotSpeed = 90.0f;

    void Start()
    {
        rotAroundPoint = Parent.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 parentLookDir = Parent.up;
        float angle = Vector2.Angle(currentLookDir, parentLookDir);
        if (angle > 180.0f)
            angle -= 360.0f;
        if (angle < 0)
            angle = Mathf.Max(angle, -rotSpeed * Time.deltaTime);
        else
            angle = Mathf.Min(angle, rotSpeed * Time.deltaTime);
        transform.RotateAround(transform.position + rotAroundPoint, Vector3.forward, angle); 
    }
}
