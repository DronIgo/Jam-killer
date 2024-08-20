using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform extraView;
    public Camera camera;

    public float cameraDefaultSize = 6f;
    public float cameraFishingSize = 4f;
    public float cameraBigFishSize = 20f;

    public float cameraDesiredSize = 6f;

    public float sizeChangeSpeed = 10f;
    public float cameraSize
    {
        get { return camera.orthographicSize; }
        set { camera.orthographicSize = value; }
    }

    public void Start()
    {
        player = GameManager.instance.player.transform;
        cameraDefaultSize = player.GetComponent<ShipController>().cameraSize;
        cameraDesiredSize = cameraDefaultSize;
    }


    private void Update()
    {
        if (extraView != null)
            transform.position = (player.position +extraView.position) / 2.0f + Vector3.forward * (-10.0f);
        else
            transform.position = player.position + Vector3.forward * (-10.0f);

        if (cameraSize != cameraDesiredSize)
        {
            float sizeUpdate = sizeChangeSpeed * Time.deltaTime * Mathf.Sign(cameraDesiredSize - cameraSize);
            if (Mathf.Abs(sizeUpdate) > Mathf.Abs(cameraDesiredSize - cameraSize))
            {
                sizeUpdate = cameraDesiredSize - cameraSize;
            }
            cameraSize += sizeUpdate;
        }
    }

}
