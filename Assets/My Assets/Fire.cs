using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Fire : MonoBehaviour
{
    public float shurikenSpeed;
    private float ShurikenCooldown = 0.1f;
    private float ShurikenRotation = 100f;
    public float teleporterSpeed;
    private float TeleporterCooldown = 2f;
    public float meleeDamage;
    public float meleeRange = 0.5f;

    public bool shurikenFired = false;
    public bool teleporterFired = false;
    private bool facingRight = true;

    public Transform ProjectileSpawnPoint;
    public Coroutine c_RShuriken;
    public Coroutine c_RTeleporter;
    public GameObject ProjectileObj;
    public GameObject TeleporterObj;
    public GameObject MeleeObj;
    public LayerMask m_EnemyLayer;

    [SerializeField] private float m_fAimingRotationSpeed;
    [SerializeField] public Transform m_tPivot;
    [SerializeField] Camera m_Cam;
    public Vector2 m_MousePos;
    public Vector2 lookDir;
    public Quaternion target;

    Rigidbody2D m_RB;
    PlayerCharacter PlayerCharacterScr;
    InputHandler InputHandlerScr;

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
            bullet.transform.rotation = target;
            StartCoroutine(PlayerCharacterScr.C_CameraShake(0.1f, 0.5f));
            bullet.GetComponent<Rigidbody2D>().AddForce(ProjectileSpawnPoint.right * shurikenSpeed, ForceMode2D.Impulse);
            //bullet.GetComponent<Rigidbody2D>().velocity = PlayerCharacterScr.FireDirection * shurikenSpeed;

            if (c_RShuriken == null)
            {
                c_RShuriken = StartCoroutine(C_Shuriken());
            }

            StartCoroutine(C_ShurikenRotation(bullet));
        }
    }

    public void FireTeleporter()
    {
        if (!teleporterFired)
        {
            var bullet = Instantiate(TeleporterObj, ProjectileSpawnPoint.position, transform.rotation);
            ProjectileTeleporter teleport = bullet.GetComponent<ProjectileTeleporter>();
            //GameObject TeleporterProjectile = GameObject.FindGameObjectWithTag("ProjectileTeleporter");
            //ProjectileTeleporter teleport = TeleporterProjectile.GetComponent<ProjectileTeleporter>();
            bullet.transform.rotation = target;
            StartCoroutine(PlayerCharacterScr.C_CameraShake(0.1f, 0.5f));
            bullet.GetComponent<Rigidbody2D>().AddForce(ProjectileSpawnPoint.right * teleporterSpeed, ForceMode2D.Impulse);

            if(c_RTeleporter == null)
            {
                c_RTeleporter = StartCoroutine(C_Teleporter());
            }
        }
    }

    private void Aim()
    {
        m_MousePos = m_Cam.ScreenToWorldPoint(Input.mousePosition);

        lookDir = m_MousePos - m_RB.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        target = Quaternion.Euler(0, 0, angle);

        if (PlayerCharacterScr.m_b_FacingRight)
        {
            m_tPivot.rotation = Quaternion.RotateTowards(m_tPivot.rotation, target, m_fAimingRotationSpeed * Time.deltaTime);
        }
        else if (!PlayerCharacterScr.m_b_FacingRight)
        {
            m_tPivot.rotation = Quaternion.RotateTowards(m_tPivot.rotation, target, m_fAimingRotationSpeed * Time.deltaTime);
        }
    }

    public void Melee()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(ProjectileSpawnPoint.position, meleeRange, m_EnemyLayer);
        
        if(hitEnemies != null)
        {
            StartCoroutine(PlayerCharacterScr.C_CameraShake(0.1f, 2f));
        }

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

    private IEnumerator C_ShurikenRotation(GameObject shuriken)
    {
        while(shurikenFired)
        {
            shuriken.transform.Rotate(Vector3.forward * Time.deltaTime * ShurikenRotation);
            yield return null;
        }
    }

    public IEnumerator C_Teleporter()
    {
        teleporterFired = true;
        yield return new WaitForSeconds(TeleporterCooldown);
        teleporterFired = false;
        c_RTeleporter = null;
    }
}

//Vector3 rotation = mousePos - transform.position;
//rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
//transform.rotation = Quaternion.Euler(0,0, rotZ);



//FIRETELEPORTER FUNCTION
//if(teleport.facingRight && !PlayerCharacterScr.m_b_FacingRight)
//{
//    teleport.FlipProjectile();
//}
//else if(!teleport.facingRight && PlayerCharacterScr.m_b_FacingRight)
//{
//    teleport.FlipProjectile();
//}

//bullet.GetComponent<Rigidbody2D>().velocity = PlayerCharacterScr.FireDirection * teleporterSpeed;
