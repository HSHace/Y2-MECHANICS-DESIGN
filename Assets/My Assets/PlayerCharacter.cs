using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerCharacter : MonoBehaviour
{
    [Header("Movement")]
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
    [SerializeField] public GameObject rasengan;

    public float m_faxis { get; set; }
    public bool m_b_FacingRight = true;
    public bool isJumping;
    public bool isWalling;
    public bool isSlaming;
    public bool isDashing;
    public bool canDash;
    public bool gravityApexStatus;
    bool jumpBufferStatus;
    bool coyoteTime;

    public ParticleSystem particleDust;
    public ParticleSystem particleDash;
    public ParticleSystem particleSlam;
    public ParticleSystem particleCircle;
    public ParticleSystem particleStar;
    public ParticleSystem particleFlash;

    [SerializeField] AudioClip[] TPSounds;
    private AudioSource PlayerAudioSource;

    Coroutine c_RJumpBuffer;
    Coroutine c_RCoyoteTime;
    public Coroutine c_RDash;

    Rigidbody2D rb;
    InputHandler InputHandler;
    GroundedComp GroundedComp;
    StaminaComponent StaminaComponentScr;
    Fire FireScr;
    CameraShake CameraShakeScr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerAudioSource = GetComponent<AudioSource>();
        GroundedComp = GetComponent<GroundedComp>();
        InputHandler = GetComponent<InputHandler>();
        StaminaComponentScr = GetComponent<StaminaComponent>();
        FireScr = GetComponent<Fire>();
        CameraShakeScr = GameObject.FindGameObjectWithTag("Camera").GetComponent<CameraShake>();
        canDash = true;
        isDashing = false;
        rasengan.SetActive(false);
    }

    private void OnEnable()
    {
        GroundedComp.OnGroundedChanged += Handle_GroundedChanged;
        //GroundedComp.OnWallChanged += Handle_WalledChanged;
    }

    private void OnDisable()
    {
        GroundedComp.OnGroundedChanged -= Handle_GroundedChanged;
        //GroundedComp.OnWallChanged -= Handle_WalledChanged;
    }

    public void Handle_GroundedChanged(bool grounded)
    {
        if (grounded)
        {
            Debug.Log("Player is grounded!");
            isJumping = false;
            isSlaming = false;
            particleSlam.Stop();
            canDash = true;
            StartCoroutine(C_CameraShake(0.1f, 1.8f));

            if (!isDashing)
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

    public void Move()
    {
        if (isDashing) return;

        ParticleDust();
        //rb.AddForce(transform.right * m_faxis * moveSpeed * 1);
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

    public void Jump()
    {
        if (isDashing) return;

        isJumping = true;
        StartCoroutine(C_JumpBlindness());
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        StartCoroutine(C_CameraShake(0.2f, 2f));
        StaminaComponentScr.StaminaDrain(5f);

        if (!GroundedComp.IsGrounded)
        {
            isJumping = true;
        }
    }

    public void Dash()
    {
        if(c_RDash == null && canDash == true)
        {
            c_RDash = StartCoroutine(C_Dash());
        }
    }

    public void PlayerJump()
    {
        if (isDashing) return;

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

    public void PlayerSlam()
    {
        if (!GroundedComp.IsGrounded && !isDashing)
        {
            StartCoroutine(C_SlamFlash(particleSlam));
            StartCoroutine(C_CameraShake(1f, 2f));
            rb.AddForce(Vector2.down * SlamForce, ForceMode2D.Impulse);
            StaminaComponentScr.StaminaDrain(15f);
            isJumping = false;
            isSlaming = true;
        }
    }

    public void Flip(GameObject player)
    {
        Vector2 currentScale = player.transform.localScale;
        currentScale.x *= -1;
        player.transform.localScale= currentScale;
        m_b_FacingRight = !m_b_FacingRight;
    }

    public void SetTeleporterLocation(Transform teleporterLocation)
    {
        StartCoroutine(C_TeleportFlash(particleCircle, particleStar, particleFlash));
        transform.position = teleporterLocation.position;
        PlayerAudioSource.PlayOneShot(TPSounds[Random.Range(0, TPSounds.Length)], 0.5f);
    }

    public void ParticleDust()
    {
        if (GroundedComp.IsGrounded)
        {
            particleDust.Play();
        }
    }

    public IEnumerator C_Dash()
    {
        isDashing = true;
        canDash = false;
        particleDash.Play();
        rb.gravityScale = 0f;
        rb.AddForce(transform.up * DashForce/6, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        Vector2 initialVelocity = rb.velocity;
        StartCoroutine(C_CameraShake(0.225f, 6f));
        rb.velocity = FireScr.ProjectileSpawnPoint.right * DashForce;
        StaminaComponentScr.StaminaDrain(15f);
        yield return new WaitForSeconds(DashTime);
        rb.velocity = new Vector2 (initialVelocity.x * m_faxis, 0);
        rb.AddForce(FireScr.ProjectileSpawnPoint.right * DashForce / 6, ForceMode2D.Impulse);
        rb.gravityScale = DefaultGravity;
        isDashing = false;
        particleDash.Stop();
        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
        c_RDash = null;
    }

    public IEnumerator C_Rasengan()
    {
        rasengan.SetActive(true);
        Debug.Log("Rasengan Active");
        yield return new WaitForSeconds(2f);
        rasengan.SetActive(false);
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

    IEnumerator C_TeleportFlash(ParticleSystem circleEffect, ParticleSystem starEffect, ParticleSystem flashEffect)
    {
        flashEffect.Play();
        circleEffect.Play();
        starEffect.Play();
        yield return new WaitForSeconds(0.1f);
        flashEffect.Stop();
        starEffect.Stop();
        circleEffect.Stop();
    }

    IEnumerator C_SlamFlash(ParticleSystem slamEffect)
    {
        slamEffect.Play();
        yield return new WaitForSeconds(1.5f);
        slamEffect.Stop();
    }

    public IEnumerator C_CameraShake(float shakeTime, float intensity)
    {
        CameraShakeScr.CameraShakeStart(intensity);
        yield return new WaitForSeconds(shakeTime);
        CameraShakeScr.CameraShakeStop();
    }
}
