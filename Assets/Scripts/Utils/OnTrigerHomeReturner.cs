using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTrigerHomeReturner : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ShipController>(out ShipController shipController))
        {
            GameManager.instance.canReturnHome = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ShipController>(out ShipController shipController))
        {
            GameManager.instance.canReturnHome = false;
        }
    }
}
