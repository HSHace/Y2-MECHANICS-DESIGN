using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Staminabar : MonoBehaviour
{
    [SerializeField] private StaminaComponent playerStamina;
    [SerializeField] private Image totalStaminabar;
    [SerializeField] private Image currentStaminabar;

    private void Awake()
    {
        currentStaminabar.fillAmount = playerStamina.m_CurrentStamina / playerStamina.m_MaxStamina;
    }

    private void Update()
    {
        currentStaminabar.fillAmount = playerStamina.m_CurrentStamina / playerStamina.m_MaxStamina;
    }   
}
