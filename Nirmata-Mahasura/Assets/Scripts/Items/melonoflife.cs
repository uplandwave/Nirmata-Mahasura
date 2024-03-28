using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Melon : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Check if the collided object has the Health script attached
            Health playerHealth = collision.GetComponent<Health>();
            
            // If the playerHealth object is not null, reset its health to 100
            if (playerHealth != null)
            {
                playerHealth.ResetHealth();
            }
            Destroy(collision.gameObject);
        }
    }
}

