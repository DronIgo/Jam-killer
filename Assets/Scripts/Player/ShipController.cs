using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float speedHorizontal = 5f;
    public float speedVertical = 5f;


    // for debug
    public Vector3 move;

    private InputManager input;

    // Start is called before the first frame update
    void Start()
    {
        input = InputManager.instance;
    }

    // Update is called once per frame
    void Update()
    {

        float moveX = input.horizontalMovement * speedHorizontal;
        float moveY = input.verticalMovement * speedVertical;

        move = new Vector3(moveX, moveY, 0f) * Time.deltaTime;

        transform.position += move;
    }
}
