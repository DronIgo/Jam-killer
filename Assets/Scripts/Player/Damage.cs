using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damageAmount = 1;

    // unity func called when a Collider2d enters any attached 2d trigger collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("NATO OnTriggerEnter2D");
        DealDamage(collision.gameObject);
    }

    // interact with health component
    private void DealDamage(GameObject collisionGameObject)
    {
        Health collidedHealth = collisionGameObject.GetComponent<Health>();
        if (collidedHealth != null)
        {
            collidedHealth.TakeDamage(damageAmount);
        }
    }

}
