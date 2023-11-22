using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public float shurikenSpeed;
    public float teleporterSpeed;
    public float meleeDamage = 25f;
    public float meleeRange = 0.5f;
    private float ShurikenCooldown = 0.25f;
    private float TeleporterCooldown = 2f;

    public bool shurikenFired = false;
    public bool teleporterFired = false;
    private bool facingRight = true;

    public LayerMask m_EnemyLayer;
    public Coroutine c_RShuriken;
    public Coroutine c_RTeleporter;
    public Transform ProjectileSpawnPoint;
    public GameObject ProjectileObj;
    public GameObject TeleporterObj;
    [SerializeField] public GameObject MeleeObj;

    [SerializeField] private GameObject[] Projectiles;

    PlayerCharacter PlayerCharacterScr;
    InputHandler InputHandlerScr;


    [SerializeField] Camera m_Cam;
    [SerializeField] Transform m_tPivot;
    [SerializeField] float m_fAimingRotationSpeed = 400f;
    private Rigidbody2D m_RB;
    Vector2 m_MousePos;





    private void Update()
    {
        Aim();
    }


    private void Awake()
    {
        m_RB = GetComponent<Rigidbody2D>();
        PlayerCharacterScr = GetComponent<PlayerCharacter>();
        InputHandlerScr = GetComponent<InputHandler>();
        MeleeObj.SetActive(false);
    }

    public void FireShuriken()
    {
        if (!shurikenFired)
        {
            var bullet = Instantiate(ProjectileObj, ProjectileSpawnPoint.position, transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = PlayerCharacterScr.FireDirection * shurikenSpeed;
            if(c_RShuriken == null)
            {
                c_RShuriken = StartCoroutine(C_Shuriken());
            }
        }
    }

    public void FireTeleporter()
    {
        if (!teleporterFired)
        {
            var bullet = Instantiate(TeleporterObj, ProjectileSpawnPoint.position, transform.rotation);
            GameObject TeleporterProjectile = GameObject.FindGameObjectWithTag("ProjectileTeleporter");
            ProjectileTeleporter teleport = TeleporterProjectile.GetComponent<ProjectileTeleporter>();

            if(teleport.facingRight && !PlayerCharacterScr.m_b_FacingRight)
            {
                teleport.FlipProjectile();
            }
            else if(!teleport.facingRight && PlayerCharacterScr.m_b_FacingRight)
            {
                teleport.FlipProjectile();
            }

            bullet.GetComponent<Rigidbody2D>().velocity = PlayerCharacterScr.FireDirection * teleporterSpeed;

            if(c_RTeleporter == null)
            {
                c_RTeleporter = StartCoroutine(C_Teleporter());
            }
        }
    }

    private void Aim()
    {
        m_MousePos = m_Cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = m_MousePos - m_RB.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        Quaternion Target = Quaternion.Euler(0, 0, angle);
        m_tPivot.rotation = Quaternion.RotateTowards(m_tPivot.rotation, Target, m_fAimingRotationSpeed * Time.deltaTime);
    }

    public void Melee()
    {
        Debug.Log("Melee performed");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ProjectileSpawnPoint.position, meleeRange, m_EnemyLayer);

        if (InputHandlerScr.m_b_InMeleeActive)
        {
            MeleeObj.SetActive(true);
        }
        else if (!InputHandlerScr.m_b_InMeleeActive)
        {
            MeleeObj.SetActive(false);
        }

        foreach (Collider2D enemy in hitEnemies) 
        {
            enemy.GetComponent<EnemyCharacter>().TakeDamage(meleeDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (ProjectileSpawnPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(ProjectileSpawnPoint.position, meleeRange);
    }
    public IEnumerator C_Shuriken()
    {
        shurikenFired = true;
        yield return new WaitForSeconds(ShurikenCooldown);
        shurikenFired = false;
        c_RShuriken = null;
    }

    public IEnumerator C_Teleporter()
    {
        teleporterFired = true;
        yield return new WaitForSeconds(TeleporterCooldown);
        teleporterFired = false;
        c_RTeleporter = null;
    }
}