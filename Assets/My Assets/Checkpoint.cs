using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    Vector2 position;

    private void Awake()
    {
        position= gameObject.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject.GetComponent<HealthComponent>();

            if (player != null)
            {
                player.RespawnPoint(position);
            }
        }  
    }
}
