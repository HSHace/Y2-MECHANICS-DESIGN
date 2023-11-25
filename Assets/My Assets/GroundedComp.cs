using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;

public class GroundedComp : MonoBehaviour
{
    private bool m_bGrounded;
    private bool m_bWalled;
    public event Action<bool> OnGroundedChanged;
    public event Action<bool> OnWallChanged;
    public bool IsGrounded { get { return m_bGrounded; } }

    [SerializeField] private Collider2D m_GroundCol;
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private LayerMask m_WallLayer;

    Rigidbody2D rb;
    InputHandler InputHandler;
    PlayerCharacter PlayerCharacterScr;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        InputHandler = GetComponent<InputHandler>();
        PlayerCharacterScr = GetComponent<PlayerCharacter>();
        if (!m_GroundCol) m_GroundCol = GetComponent<Collider2D>();
    }

    public void Update()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.layerMask = m_GroundLayer;
        List<RaycastHit2D> results = new List<RaycastHit2D>();
        bool newGrounded = (m_GroundCol.Cast(Vector2.down, filter, results, 0.1f, true) > 0);

        if (m_bGrounded != newGrounded)
        {
            m_bGrounded = newGrounded;
            OnGroundedChanged?.Invoke(m_bGrounded);
        }

        //STICKY FEET
        if (InputHandler.c_RMove == null && IsGrounded == true)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }
}