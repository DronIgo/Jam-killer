using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveOnTrigger : MonoBehaviour
{
    public Transform center;
    public float minDistanceX;
    public float minDistanceY;


    public Vector3 setDirection;
    public float durationOfMovement;
    public bool useSetDirection;

    private ShipController ship;
    private Transform shipTransform;

    public UnityEvent doAfterMovement;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ShipController>(out ShipController shipController))
        {
            ship = shipController;
            shipTransform = collision.gameObject.transform;
            StartCoroutine("MoveShip");
        }
    }

    private IEnumerator MoveShip()
    {
        ship.enabled = false;
        float timePassed = 0;
        Vector3 endPos;
        Vector3 startPos = shipTransform.position;
        if (useSetDirection)
            endPos = startPos + setDirection;
        else
        {
            Vector3 dirFromCenter = startPos - center.position;
            Vector3 move = new Vector3(dirFromCenter.normalized.x * minDistanceX, dirFromCenter.normalized.y * minDistanceY, 0);
            endPos = center.position + move;
        }
        while (timePassed < durationOfMovement)
        {
            timePassed += Time.deltaTime;
            if (timePassed >= durationOfMovement)
                timePassed = durationOfMovement;

            shipTransform.position = Vector3.Lerp(startPos, endPos, timePassed / durationOfMovement);
            yield return new WaitForEndOfFrame();
        }
        ship.enabled = true;
        ship.currentVelocity = new Vector2(0, 0);
        doAfterMovement.Invoke();
    }
}
