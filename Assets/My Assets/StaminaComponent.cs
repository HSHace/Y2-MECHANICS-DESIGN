using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaComponent : MonoBehaviour
{
    public bool canMove;
    public bool m_b_InStaminaRegen;
    public float m_CurrentStamina;

    public float m_MaxStamina = 100.0f;
    private float idleRecovery = 0.5f;

    Coroutine c_RStaminaRegen;
    Coroutine c_RBackUpStamina;
    InputHandler InputHandlerScr;
    HealthComponent HealthComponentScr;

    private void Awake()
    {
        InputHandlerScr = GetComponent<InputHandler>();
        HealthComponentScr = GetComponent<HealthComponent>();
        m_CurrentStamina = m_MaxStamina;
        canMove = true;
    }

    public void StaminaDrain(float stamina)
    {
        m_CurrentStamina = Mathf.Clamp(m_CurrentStamina - stamina, 0, m_MaxStamina);

        if(c_RStaminaRegen == null)
        {
            c_RStaminaRegen = StartCoroutine(C_StaminaRegeneration());
        }
        else if(c_RStaminaRegen !=null)
        {
            StopCoroutine(c_RStaminaRegen);
            c_RStaminaRegen = StartCoroutine(C_StaminaRegeneration());
        }

        canMove = (m_CurrentStamina > 0);

        if(HealthComponentScr.c_RHealthRegeneration == null)
        {
            HealthComponentScr.c_RHealthRegeneration = StartCoroutine(HealthComponentScr.C_HealthRegeneration());
        }

        if(c_RBackUpStamina == null)
        {
            StartCoroutine(C_BackUpStamina());
        }
    }
    public void StaminaRegeneration(float stamina)
    {
        m_CurrentStamina = Mathf.Clamp(m_CurrentStamina + stamina, 0, m_MaxStamina);
    }

    public IEnumerator C_StaminaRegeneration()
    {
        m_b_InStaminaRegen = true;

        while (InputHandlerScr.m_b_Idle && m_CurrentStamina < m_MaxStamina)
        {
            StaminaRegeneration(idleRecovery);
            yield return new WaitForSeconds(0.25f);
            idleRecovery += idleRecovery * 1.01f;
        }

        c_RStaminaRegen = null;
        m_b_InStaminaRegen = false;
        idleRecovery = 0.5f;
    }

    private IEnumerator C_BackUpStamina()
    {
        while (m_CurrentStamina != m_MaxStamina)
        {
            yield return new WaitForSeconds(3f);

            if (c_RStaminaRegen == null)
            {
                c_RStaminaRegen = StartCoroutine(C_StaminaRegeneration());
            }
        }
    }
}
