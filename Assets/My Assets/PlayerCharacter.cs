using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] float SlamForce;
    [SerializeField] float circleRadius;
    [SerializeField] float MaxMoveSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float DefaultGravity;
    [SerializeField] public float ApexGravity;
    [SerializeField] public float FallGravity;
    [SerializeField] public float JumpGravity;
    [SerializeField] private float CoyoteTimer;
    [SerializeField] private float DashForce;
    [SerializeField] private float DashTime;
    [SerializeField] private float DashCooldown;
    [SerializeField] public LayerMask m_LayerMask;
    [SerializeField] Transform castPosition;
    [SerializeField] GameObject luffy;

    public float m_faxis { get; set; }
    public bool m_b_FacingRight = true;
    public bool isJumping;
    public bool isSlaming;
    public bool isDashing;
    public bool canDash;
    public Vector2 FireDirection;

    public bool gravityApexStatus;
    bool jumpBufferStatus;
    bool coyoteTime;

    Coroutine c_RJumpBuffer;
    Coroutine c_RCoyoteTime;
    public Coroutine c_RDash;

    Rigidbody2D rb;
    InputHandler InputHandler;
    GroundedComp GroundedComp;
    StaminaComponent StaminaComponentScr;
    Fire FireScr;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GroundedComp = GetComponent<GroundedComp>();
        InputHandler = GetComponent<InputHandler>();
        StaminaComponentScr = GetComponent<StaminaComponent>();
        FireScr = GetComponent<Fire>();
        canDash = true;
    }

    private void OnEnable()
    {
        GroundedComp.OnGroundedChanged += Handle_GroundedChanged;
    }

    private void OnDisable()
    {
        GroundedComp.OnGroundedChanged -= Handle_GroundedChanged;
    }

    public void Handle_GroundedChanged(bool grounded)
    {
        if (grounded)
        {
            Debug.Log("Player is grounded!");
            isJumping = false;
            isSlaming = false;
            canDash = true;

            if(!isDashing)
            {
                rb.gravityScale = DefaultGravity;
            }

            if (jumpBufferStatus)
            {
                StopCoroutine(c_RJumpBuffer);
                jumpBufferStatus = false;
                Jump();
            }

            if(InputHandler.c_RGravityApex != null)
            {
                StopCoroutine(InputHandler.c_RGravityApex);
                InputHandler.c_RGravityApex = null;
                gravityApexStatus = false;
            }
        }
        else if (!jumpBufferStatus)
        {
            c_RCoyoteTime = StartCoroutine(C_CoyoteTime());
        }
    }

    public void PlayerJump()
    {
        if(!isDashing)
        {
            if (GroundedComp.IsGrounded)
            {
                Jump();
            }
            else if (coyoteTime)
            {
                Jump();
                StopCoroutine(c_RCoyoteTime);
                coyoteTime = false;
            }
            else if (!GroundedComp.IsGrounded)
            {
                c_RJumpBuffer = StartCoroutine(C_JumpBuffer());
            }
        }
    }

    public void PlayerSlam()
    {
        rb.AddForce(Vector2.down * SlamForce, ForceMode2D.Impulse);
        StaminaComponentScr.StaminaDrain(15f);
        isJumping = false;
        isSlaming = true;
    }

    public void Move()
    {
        if (!isDashing)
        {
            rb.AddForce(transform.right * m_faxis * moveSpeed * 1);
            rb.velocity = new Vector2(m_faxis * moveSpeed, rb.velocity.y);
            StaminaComponentScr.StaminaDrain(0.2f);

            //Debug.Log($"Axis: {m_faxis} ");

            if (m_faxis > 0 && !m_b_FacingRight)
            {
                Flip(luffy);
            }
            else if (m_faxis < 0 && m_b_FacingRight)
            {
                Flip(luffy);
            }
        }
    }

    public void Jump()
    {
        if(!isDashing)
        {
            StartCoroutine(C_JumpBlindness());
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StaminaComponentScr.StaminaDrain(5f);

            if (!GroundedComp.IsGrounded)
            {
                isJumping = true;
            }
        } 
    }

    public void Dash()
    {
        if(c_RDash == null && canDash == true)
        {
            c_RDash = StartCoroutine(C_Dash());
        }
    }

    public void Flip(GameObject luffy)
    {
        Vector2 currentScale = luffy.transform.localScale;
        currentScale.x *= -1;
        luffy.transform.localScale= currentScale;
        m_b_FacingRight = !m_b_FacingRight;
    }

    public void SetTeleporterLocation(Transform teleporterLocation)
    {
        transform.position = teleporterLocation.position;
    }

    public IEnumerator C_Dash()
    {
        isDashing = true;
        canDash = false;
        rb.gravityScale = 0f;
        rb.AddForce(transform.up * DashForce/6, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        var initialVelocity = rb.velocity;
        StaminaComponentScr.StaminaDrain(15f);
        rb.velocity = FireScr.ProjectileSpawnPoint.right * DashForce;
        yield return new WaitForSeconds(DashTime);
        rb.velocity = new Vector2 (initialVelocity.x, 0);
        rb.AddForce(FireScr.ProjectileSpawnPoint.right * DashForce/6, ForceMode2D.Impulse);
        rb.gravityScale = DefaultGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashCooldown);
        Debug.Log("DASH COOLDOWN DONE!!");
        canDash = true;
        c_RDash = null;
    }

    public IEnumerator C_JumpBuffer()
    {
        jumpBufferStatus = true;
        yield return new WaitForSeconds(0.35f);
        jumpBufferStatus = false;
    }

    public IEnumerator C_CoyoteTime()
    {
        coyoteTime = true;
        yield return new WaitForSeconds(CoyoteTimer);
        coyoteTime = false;
    }

    public IEnumerator C_GravityApex()
    {
        rb.gravityScale = JumpGravity;
        while (InputHandler.m_b_InJumpActive && !gravityApexStatus)
        {
            if(rb.velocity.y <= 1)
            {
                rb.gravityScale = ApexGravity;
                gravityApexStatus = true;
            }

            yield return null;
        }

        yield return new WaitForSeconds(.2f);
        rb.gravityScale = FallGravity;
    }

    IEnumerator C_JumpBlindness()
    {
        jumpBufferStatus = true;
        yield return new WaitForSeconds(0.2f);
        jumpBufferStatus = false;
    }
}
