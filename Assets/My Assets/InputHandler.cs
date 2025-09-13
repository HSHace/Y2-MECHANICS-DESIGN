using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] PlayerInput m_PlayerInput;

    public bool m_b_Idle;
    public bool m_b_InMoveActive;
    public bool m_b_InJumpActive;
    public bool m_b_InSlamActive;
    public bool m_b_InDashActive;
    public bool m_b_InFiredActive;
    public bool m_b_InMeleeActive;
    public bool m_b_InTeleporterActive;

    public Coroutine c_RMove;
    public Coroutine c_RJump;
    public Coroutine c_RSlam;
    public Coroutine c_RGravityApex;
    public Coroutine c_RRasengan;

    Rigidbody2D rb;
    PlayerCharacter PlayerCharacterScr;
    GroundedComp GroundedComp;
    HealthComponent HealthComponentScr;
    StaminaComponent StaminaComponentScr;
    Fire FireScr;
    ProjectileTeleporter ProjectileTeleporterScr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        m_PlayerInput = GetComponent<PlayerInput>();
        PlayerCharacterScr = GetComponent<PlayerCharacter>();
        HealthComponentScr = GetComponent<HealthComponent>();
        StaminaComponentScr = GetComponent<StaminaComponent>();
        FireScr = GetComponent<Fire>();
        ProjectileTeleporterScr = GetComponent<ProjectileTeleporter>();
    }

    private void OnEnable()
    {
        m_PlayerInput.actions.FindAction("Movement").performed += Handle_MovePerformed;
        m_PlayerInput.actions.FindAction("Movement").canceled += Handle_MoveCancelled;
        m_PlayerInput.actions.FindAction("Jump").performed += Handle_JumpPerformed;
        m_PlayerInput.actions.FindAction("Jump").canceled += Handle_JumpCancelled;
        m_PlayerInput.actions.FindAction("Slam").performed += Handle_SlamPerformed;
        m_PlayerInput.actions.FindAction("Slam").canceled += Handle_SlamCancelled;
        m_PlayerInput.actions.FindAction("Dash").performed += Handle_DashPerformed;
        m_PlayerInput.actions.FindAction("Dash").canceled += Handle_DashCancelled;

        m_PlayerInput.actions.FindAction("Fire").performed += Handle_ProjectileFired;
        m_PlayerInput.actions.FindAction("Teleporter").performed += Handle_TeleporterPerformed;
        m_PlayerInput.actions.FindAction("Teleporter").canceled += Handle_TeleporterCancelled;
        m_PlayerInput.actions.FindAction("Rasengan").performed += Handle_RasenganPerformed;
        m_PlayerInput.actions.FindAction("Melee").performed += Handle_MeleePerformed;
        m_PlayerInput.actions.FindAction("Melee").canceled += Handle_MeleeCancelled;

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
        m_PlayerInput.actions.FindAction("Dash").performed -= Handle_DashPerformed;
        m_PlayerInput.actions.FindAction("Dash").canceled -= Handle_DashCancelled;

        m_PlayerInput.actions.FindAction("Fire").performed -= Handle_ProjectileFired;
        m_PlayerInput.actions.FindAction("Teleporter").performed -= Handle_TeleporterPerformed;
        m_PlayerInput.actions.FindAction("Teleporter").canceled -= Handle_TeleporterCancelled;
        m_PlayerInput.actions.FindAction("Melee").performed -= Handle_MeleePerformed;
        m_PlayerInput.actions.FindAction("Melee").canceled -= Handle_MeleeCancelled;

        m_PlayerInput.actions.FindAction("Damage").performed -= Handle_DamagePerformed;
    }

    private void Handle_MovePerformed(InputAction.CallbackContext context)
    {
        PlayerCharacterScr.m_faxis = context.ReadValue<float>();
        m_b_InMoveActive = true;
        m_b_Idle = false;
        if (c_RMove == null)
        {
            c_RMove = StartCoroutine(C_MoveUpdate());
        }
    }

    private void Handle_MoveCancelled(InputAction.CallbackContext context)
    {
        PlayerCharacterScr.m_faxis = context.ReadValue<float>();
        m_b_InMoveActive = false;
        m_b_Idle = true;
        if (c_RMove != null)
        {
            StopCoroutine(c_RMove);
            c_RMove = null;
        }

        PlayerCharacterScr.Move();
    }

    private void Handle_JumpPerformed(InputAction.CallbackContext context)
    {
        m_b_InJumpActive = true;
        m_b_Idle = false;

        if (c_RJump == null)
        {
            c_RJump = StartCoroutine(C_JumpUpdate());
        }

        if(c_RGravityApex == null) 
        {
            c_RGravityApex = StartCoroutine(PlayerCharacterScr.C_GravityApex());
        }
    }

    private void Handle_JumpCancelled(InputAction.CallbackContext context) 
    {
        m_b_InJumpActive = false;
        m_b_Idle = true;

        rb.gravityScale = PlayerCharacterScr.FallGravity;

        if (c_RJump!= null)
        {
            StopCoroutine(c_RJump);
            c_RJump = null;
        }

        if(c_RGravityApex!= null)
        {
            StopCoroutine(c_RGravityApex);
            PlayerCharacterScr.gravityApexStatus = false;
        }
    }

    private void Handle_SlamPerformed(InputAction.CallbackContext context)
    {
        m_b_InSlamActive = true;

        if (c_RSlam == null) 
        {
           c_RSlam = StartCoroutine(C_SlamUpdate());
        }

        m_b_Idle = false;
    }

    private void Handle_SlamCancelled(InputAction.CallbackContext context) 
    {
        m_b_InSlamActive =false;
        m_b_Idle = true;

        if (c_RSlam != null)
        {
            StopCoroutine(c_RSlam);
            c_RSlam = null;
        }
    }

    private void Handle_DashPerformed(InputAction.CallbackContext context)
    {
        m_b_InDashActive = true;
        PlayerCharacterScr.Dash();
    }

    private void Handle_DashCancelled(InputAction.CallbackContext context)
    {
        m_b_InDashActive = false;
    }

    private void Handle_ProjectileFired(InputAction.CallbackContext context)
    {
        m_b_InFiredActive = true;
        FireScr.FireShuriken();
    }

    private void Handle_TeleporterPerformed(InputAction.CallbackContext context)
    {
        FireScr.FireTeleporter();
        m_b_InTeleporterActive = true;
    }

    private void Handle_TeleporterCancelled(InputAction.CallbackContext context)
    {
        if (GameObject.FindGameObjectWithTag("ProjectileTeleporter") != null)
        {
            GameObject TeleporterProjectile = GameObject.FindGameObjectWithTag("ProjectileTeleporter");
            ProjectileTeleporter teleport = TeleporterProjectile.GetComponent<ProjectileTeleporter>();
            teleport.TeleportPlayer();
            m_b_InTeleporterActive = false;
        }
    }

    private void Handle_MeleePerformed(InputAction.CallbackContext context)
    {
        m_b_InMeleeActive = true;
        FireScr.Melee();
    }

    private void Handle_MeleeCancelled(InputAction.CallbackContext context)
    {
        m_b_InMeleeActive = false;
        FireScr.MeleeObj.SetActive(false);
    }

    private void Handle_RasenganPerformed(InputAction.CallbackContext context)
    {
        if (c_RRasengan == null)
        {
            c_RRasengan = StartCoroutine(PlayerCharacterScr.C_Rasengan());
        }
        else if(c_RRasengan != null)
        {
            StopCoroutine(c_RRasengan);
            c_RRasengan = null;
            PlayerCharacterScr.rasengan.SetActive(false);
        }
    }

    private void Handle_DamagePerformed(InputAction.CallbackContext context)
    {
        HealthComponentScr.TakeDamage(10);
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
        while(m_b_InSlamActive && StaminaComponentScr.canMove)
        {
            PlayerCharacterScr.PlayerSlam();
            yield return new WaitForSeconds(1f);
        }
    }
}