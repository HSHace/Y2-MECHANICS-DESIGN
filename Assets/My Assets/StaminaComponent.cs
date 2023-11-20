using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaComponent : MonoBehaviour
{
    public float m_CurrentStamina;
    public float m_MaxStamina = 100.0f;
    public bool canMove;
    public bool m_b_InStaminaRegen;

    private float idleRecovery = 0.5f;

    Coroutine c_RStaminaDrain;

    PlayerCharacter PlayerCharacterScr;
    InputHandler InputHandlerScr;

    private void Awake()
    {
        PlayerCharacterScr = GetComponent<PlayerCharacter>();
        InputHandlerScr = GetComponent<InputHandler>();
        m_CurrentStamina = m_MaxStamina;
        canMove = true;
    }

    public void StaminaDrain(float stamina)
    {
        m_CurrentStamina = Mathf.Clamp(m_CurrentStamina - stamina, 0, m_MaxStamina);

        if(c_RStaminaDrain == null)
        {
            c_RStaminaDrain = StartCoroutine(C_StaminaRegeneration());
        }
        else if(c_RStaminaDrain !=null)
        {
            StopCoroutine(c_RStaminaDrain);
            c_RStaminaDrain = StartCoroutine(C_StaminaRegeneration());
        }

        canMove = (m_CurrentStamina > 0);
        //if(m_CurrentStamina <= 0)
        //{
        //    canMove = false;
        //}
        //else if (m_CurrentStamina > 0)
        //{
        //    canMove = true;
        //}
    }
    public void StaminaRegeneration(float stamina)
    {
        m_CurrentStamina = Mathf.Clamp(m_CurrentStamina + stamina, 0, m_MaxStamina);
        stamina += 1f;
        //currentStamina += stamina * Time.deltaTime;

        Debug.Log("Stamina recovered");
    }

    public IEnumerator C_StaminaRegeneration()
    {
        m_b_InStaminaRegen = true;

        while (InputHandlerScr.m_b_Idle && m_CurrentStamina < m_MaxStamina)
        {
            StaminaRegeneration(idleRecovery);
            yield return new WaitForSeconds(0.5f);
            idleRecovery += idleRecovery * 1.05f;
        }

        m_b_InStaminaRegen = false;
        idleRecovery = 0.5f;
        c_RStaminaDrain = null;
    }
}
