using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] public float EnemyCurrentHealth;
    [SerializeField] private float EnemyMaxHealth;

    private bool enemyDead;

    private void Awake()
    {
        EnemyCurrentHealth = EnemyMaxHealth;
    }

    public void TakeDamage(float damage)
    {
        EnemyCurrentHealth -= damage;

        if(EnemyCurrentHealth <= 0)
        {
            enemyDead = true;
            Die();
        }
    }
    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        gameObject.SetActive(false);
        enemyDead = true;
    }
}
