using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] public float HealthRegenRate;
    [SerializeField] public float maxHealth = 100.0f;
    public float currentHealth { get; private set; }
    public bool dead;
    private bool m_b_InHeal;

    public Vector2 respawnPoint;
    public Coroutine c_RHealthRegeneration;

    Rigidbody2D rb;
    BoxCollider2D collider2d;
    StaminaComponent StaminaComponentScr;

    private void Awake()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<BoxCollider2D>();
        StaminaComponentScr = GetComponent<StaminaComponent>();
        respawnPoint = gameObject.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Damage(10f);
        }
    }

    public void RespawnPoint(Vector2 position)
    {
        respawnPoint = position;
    }

    public void Damage(float damage)
    {
        Debug.Log("Damage taken");
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        if(currentHealth <= 0)
        {
            dead = true;
            Die();
        }

        if(c_RHealthRegeneration == null)
        {
            c_RHealthRegeneration = StartCoroutine(C_HealthRegeneration());
        }
    }

    public void Heal(float heal)
    {
        currentHealth = Mathf.Clamp(currentHealth + heal, 0, maxHealth);
    }

    public void Die()
    {
        currentHealth = 0f;
        collider2d = GetComponent<BoxCollider2D>();
        collider2d.enabled = false;
        rb.velocity = new Vector2(rb.velocity.x, 10f);
        StartCoroutine(Respawn());
    }

    public IEnumerator C_HealthRegeneration()
    {
        m_b_InHeal = true;
        while (StaminaComponentScr.m_CurrentStamina >= StaminaComponentScr.m_MaxStamina /1.5f)
        {
            Heal(HealthRegenRate);
            yield return new WaitForSeconds(0.5f);
        }

        m_b_InHeal = false;
        c_RHealthRegeneration = null;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1f);
        transform.position = respawnPoint;
        StaminaComponentScr.m_CurrentStamina = StaminaComponentScr.m_MaxStamina;
        collider2d.enabled = true;
        dead = false;
        currentHealth = maxHealth;
    }
}
