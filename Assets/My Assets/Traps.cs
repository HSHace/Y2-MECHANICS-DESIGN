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
        }
    }
}
