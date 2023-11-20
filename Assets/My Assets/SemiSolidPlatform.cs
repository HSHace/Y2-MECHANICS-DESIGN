using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SemiSolidPlatform : MonoBehaviour
{
    private BoxCollider2D m_collider;
    private bool playerOnPlatform;

    InputHandler InputHandlerScr;

    private void Awake()
    {
        m_collider = GetComponent<BoxCollider2D>();
        InputHandlerScr = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (InputHandlerScr.m_b_InSlamActive)
        {
            m_collider.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(1f);
        m_collider.enabled = true;
    }
}
