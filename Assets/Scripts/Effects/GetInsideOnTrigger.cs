using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInsideOnTrigger : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {

        }
    }
}
