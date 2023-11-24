using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public float HealthRegenRate;
    [SerializeField] public float maxHealth = 100.0f;
    public float currentHealth { get; private set; }
    public bool dead;
    private bool m_b_InHeal;

    public Coroutine c_RHealthRegeneration;
    StaminaComponent StaminaComponentScr;

    private void Awake()
    {
        currentHealth = maxHealth;
        StaminaComponentScr = GetComponent<StaminaComponent>();
    }

    public void Damage(float damage)
    {
        Debug.Log("Damage taken");
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if(currentHealth <= 0)
        {
            dead = true;
            GetComponent<Healthbar>().enabled = false;
        }

        if(c_RHealthRegeneration == null)
        {
            c_RHealthRegeneration = StartCoroutine(C_HealthRegeneration());
        }
    }

    public void Heal(float heal)
    {
        currentHealth = Mathf.Clamp(currentHealth + heal, 0, maxHealth);
    }

    public IEnumerator C_HealthRegeneration()
    {
        m_b_InHeal = true;
        while (StaminaComponentScr.m_CurrentStamina >= StaminaComponentScr.m_MaxStamina /1.5f)
        {
            Heal(HealthRegenRate);
            yield return new WaitForSeconds(0.5f);
        }

        m_b_InHeal = false;
        c_RHealthRegeneration = null;
    }
}
