using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private HealthComponent playerHealth;
    [SerializeField] private EnemyCharacter enemyHealth;
    [SerializeField] private Image totalHealthbar;
    [SerializeField] private Image currentHealthbar;

    private void Awake()
    {
        if(playerHealth != null)
        {
            currentHealthbar.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
        }
        else if(enemyHealth != null)
        {
            currentHealthbar.fillAmount = enemyHealth.currentEnemyHealth / enemyHealth.maxEnemyHealth;
        }
    }

    private void Update()
    {
        if(playerHealth != null)
        {
            currentHealthbar.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
        }
        else if(enemyHealth != null)
        {
            currentHealthbar.fillAmount = enemyHealth.currentEnemyHealth / enemyHealth.maxEnemyHealth;
        }
    }
}
