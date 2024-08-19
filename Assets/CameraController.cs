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

    private float cameraDesiredSize = 6f;

    public float sizeChangeSpeed = 10f;
    public float cameraSize
    {
        get { return camera.orthographicSize; }
        set { camera.orthographicSize = value; }
    }

    public void Start()
    {
        player = GameManager.instance.player.transform;
    }


    private void Update()
    {
        if (extraView != null)
            transform.position = (player.position +extraView.position) / 2.0f + Vector3.forward * (-10.0f);
        else
            transform.position = player.position + Vector3.forward * (-10.0f);
    }

}
