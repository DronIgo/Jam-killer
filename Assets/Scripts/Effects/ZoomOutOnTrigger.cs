using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOutOnTrigger : MonoBehaviour
{

    public float zoomOut = 25f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ShipController>(out ShipController shipController))
        {
            if (GameManager.instance != null)
                if (GameManager.instance.cameraController != null)
                {
                    GameManager.instance.cameraController.cameraDesiredSize = zoomOut;
                }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ShipController>(out ShipController shipController))
        {
            if (GameManager.instance != null)
                if (GameManager.instance.cameraController != null)
                {
                    GameManager.instance.cameraController.cameraDesiredSize = GameManager.instance.cameraController.cameraDefaultSize;
                }
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
            if (GameManager.instance.cameraController != null)
            {
                GameManager.instance.cameraController.cameraDesiredSize = GameManager.instance.cameraController.cameraDefaultSize;
            }
    }
}
