using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] PlayerInput m_PlayerInput;

    public bool m_b_InJumpActive;
    public bool m_b_InMoveActive;
    public bool m_b_InSlamActive;
    public bool m_b_InFiredActive;
    public bool m_b_Idle;

    public Coroutine c_RMove;
    public Coroutine c_RJump;
    public Coroutine c_RSlam;
    public Coroutine c_RGravityApex;

    Rigidbody2D rb;

    PlayerCharacter PlayerCharacterScr;
    GroundedComp GroundedComp;
    HealthComponent HealthComponentScr;
    StaminaComponent StaminaComponentScr;
    Fire FireScr;

    private void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        m_PlayerInput = GetComponent<PlayerInput>();
        PlayerCharacterScr = GetComponent<PlayerCharacter>();
        HealthComponentScr = GetComponent<HealthComponent>();
        StaminaComponentScr = GetComponent<StaminaComponent>();
        FireScr = GetComponent<Fire>();

    }

    private void OnEnable()
    {
        m_PlayerInput.actions.FindAction("Movement").performed += Handle_MovePerformed;
        m_PlayerInput.actions.FindAction("Movement").canceled += Handle_MoveCancelled;

        m_PlayerInput.actions.FindAction("Jump").performed += Handle_JumpPerformed;
        m_PlayerInput.actions.FindAction("Jump").canceled += Handle_JumpCancelled;

        m_PlayerInput.actions.FindAction("Slam").performed += Handle_SlamPerformed;
        m_PlayerInput.actions.FindAction("Slam").canceled += Handle_SlamCancelled;

        m_PlayerInput.actions.FindAction("Fire").performed += Handle_ProjectileFired;
        m_PlayerInput.actions.FindAction("Damage").performed += Handle_DamagePerformed;
    }

    private void OnDisable()
    {
        m_PlayerInput.actions.FindAction("Movement").performed -= Handle_MovePerformed;
        m_PlayerInput.actions.FindAction("Movement").canceled -= Handle_MoveCancelled;

        m_PlayerInput.actions.FindAction("Jump").performed -= Handle_JumpPerformed;
        m_PlayerInput.actions.FindAction("Jump").canceled -= Handle_JumpCancelled;

        m_PlayerInput.actions.FindAction("Slam").performed -= Handle_SlamPerformed;
        m_PlayerInput.actions.FindAction("Slam").canceled -= Handle_SlamCancelled;
        //m_PlayerInput.actions.FindAction("Slam").performed -= playerScr.PlayerSlam;
    }

    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        PlayerCharacterScr.m_faxis = context.ReadValue<float>();
        m_b_InMoveActive = true;
        if (c_RMove == null)
        {
            c_RMove = StartCoroutine(C_MoveUpdate());
        }

        m_b_Idle = false;
    }

    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        PlayerCharacterScr.m_faxis = context.ReadValue<float>();
        m_b_InMoveActive = false;
        if (c_RMove != null)
        {
            StopCoroutine(c_RMove);
            c_RMove = null;
        }

        PlayerCharacterScr.Move();
        m_b_Idle = true;
    }

    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        m_b_InJumpActive = true;

        if (c_RJump == null)
        {
            c_RJump = StartCoroutine(C_JumpUpdate());
        }

        if(c_RGravityApex == null) 
        {
            c_RGravityApex = StartCoroutine(PlayerCharacterScr.C_GravityApex());
        }

        m_b_Idle = false;
    }

    private void Handle_JumpCancelled(InputAction.CallbackContext context) 
    {
        m_b_InJumpActive = false;
        rb.gravityScale = PlayerCharacterScr.FallGravity;

        if(c_RJump!= null)
        {
            StopCoroutine(c_RJump);
            c_RJump = null;
        }

        if(c_RGravityApex!= null)
        {
            StopCoroutine(c_RGravityApex);
            rb.gravityScale = PlayerCharacterScr.FallGravity;
        }

        m_b_Idle = true;
    }

    private void Handle_SlamPerformed(InputAction.CallbackContext context)
    {
        m_b_InSlamActive = true;

        if(c_RSlam == null) 
        {
           c_RSlam = StartCoroutine(C_SlamUpdate());
        }

        m_b_Idle = false;
    }

    private void Handle_SlamCancelled(InputAction.CallbackContext context) 
    {
        m_b_InSlamActive =false;
        Debug.Log("SLAM CANCELLED");

        if (c_RSlam != null)
        {
            StopCoroutine(c_RSlam);
            c_RSlam = null;
        }

        if (GroundedComp.IsGrounded)
        {
            PlayerCharacterScr.isSlaming = false;
        }

        m_b_Idle = true;
    }

    private void Handle_ProjectileFired(InputAction.CallbackContext context)
    {
        m_b_InFiredActive = true;

        FireScr.FireShuriken();
    }

    private void Handle_DamagePerformed(InputAction.CallbackContext context)
    {
        HealthComponentScr.Damage(10);
    }

    IEnumerator C_MoveUpdate()
    {
        while (m_b_InMoveActive && StaminaComponentScr.canMove)
        {
            PlayerCharacterScr.Move();
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator C_JumpUpdate()
    {
        PlayerCharacterScr.PlayerJump();
        yield return new WaitForSeconds(.1f);
    }

    IEnumerator C_SlamUpdate() 
    {
        while(m_b_InSlamActive)
        {
            PlayerCharacterScr.PlayerSlam();
            yield return new WaitForSeconds(0.1f);
        }
    }
}