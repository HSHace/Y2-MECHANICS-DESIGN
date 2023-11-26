using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    [SerializeField] private float SpikeDamage;

    HealthComponent HealthComponentScr;

    private void Awake()
    {
        HealthComponentScr = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            HealthComponentScr.Damage(SpikeDamage);
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            PlayerCharacter player = collision.gameObject.GetComponent<PlayerCharacter>();
            rb.velocity = new Vector2(rb.velocity.x, 7.5f);
            StartCoroutine(player.C_CameraShake(0.2f, 5f));
        }
    }
}
