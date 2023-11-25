using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileTeleporter : MonoBehaviour
{
    [SerializeField] public float TeleporterDamage;
    public Transform TeleporterLocation;
    public bool facingRight = true;

    Rigidbody2D rb;
    PlayerCharacter PlayerCharacterScr;
    InputHandler InputHandlerScr;
    Fire FireScr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerCharacterScr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        InputHandlerScr = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
        FireScr = GameObject.FindGameObjectWithTag("Player").GetComponent<Fire>();
        StartCoroutine(C_Destroy());
    }

    private void Update()
    {

        TeleporterLocation = transform;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Walls"))
        {
            StickToSurface(collision.contacts[0].point);
            transform.parent = collision.transform;
            transform.position = TeleporterLocation.position;

            EnemyCharacter enemyCharacter = collision.gameObject.GetComponent<EnemyCharacter>();
            if (enemyCharacter != null)
            {
                enemyCharacter.TakeDamage(TeleporterDamage);
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            StickToSurface(collision.contacts[0].point);
        }

        gameObject.GetComponent<TrailRenderer>().enabled = false;
    }

    private void StickToSurface(Vector2 contactPoint)
    {
        transform.position = contactPoint;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;
    }

    public void TeleportPlayer()
    {
        PlayerCharacterScr.SetTeleporterLocation(TeleporterLocation);
        gameObject.GetComponent<TrailRenderer>().enabled = true;
        Destroy(gameObject);
        InputHandlerScr.m_b_InTeleporterActive = false;
    }

    public void FlipProjectile()
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        gameObject.transform.localScale = newScale;
        facingRight = !facingRight;
    }

    IEnumerator C_Destroy()
    {
        gameObject.GetComponent<TrailRenderer>().enabled = true;
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
        InputHandlerScr.m_b_InTeleporterActive = false;
    }
}