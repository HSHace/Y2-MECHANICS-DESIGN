using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private HealthComponent playerHealth;
    [SerializeField] private Image totalHealthbar;
    [SerializeField] private Image currentHealthbar;

    private void Awake()
    {
        currentHealthbar.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
    }

    private void Update()
    {
        currentHealthbar.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
