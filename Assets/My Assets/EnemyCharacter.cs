using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] public float currentEnemyHealth;
    [SerializeField] public float maxEnemyHealth = 100.0f;
    [SerializeField] Material flashMaterial;
    [SerializeField] GameObject DamageIndicator;
    [SerializeField] BoxCollider2D triggerBox;

    private bool enemyRight;
    private bool playerDetected;

    Vector2 initialVelocity;
    Vector2 target;
    Color defaultColor;

    SpriteRenderer enemySprite;
    Material defaultMaterial;
    Coroutine c_REnemyMove;

    PlayerCharacter PlayerCharacterScr;
    Rigidbody2D rb;

    private void Awake()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        PlayerCharacterScr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        rb = GetComponent<Rigidbody2D>();
        currentEnemyHealth = maxEnemyHealth;
        defaultColor = enemySprite.color;
        defaultMaterial = enemySprite.material;
        enemyRight = true;
    }

    public void Move()
    {
        initialVelocity = new Vector2(rb.velocity.x, rb.velocity.y);

        Vector2 target = new Vector2(PlayerCharacterScr.transform.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, 10f * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    public void TakeDamage(float damage)
    {
        EnemyKnockback();
        currentEnemyHealth -= damage;
        StartCoroutine(C_SpriteFlash());
        ShowDamageIndicator(-damage);
        StartCoroutine(PlayerCharacterScr.C_CameraShake(0.2f, 2f));

        if (currentEnemyHealth <= 0)
        {
            Die();
        }
    }

    void ShowDamageIndicator(float damage)
    {
        if (DamageIndicator != null)
        {
            GameObject damageIndicator = Instantiate(DamageIndicator, transform.position, Quaternion.identity);
            PopUpText popUpText = damageIndicator.GetComponent<PopUpText>();

            if (popUpText != null)
            {
                popUpText.ShowText(damage);
            }
            else
            {
                Debug.LogWarning("Damage indicator prefab is missing PopUpText component.");
            }
        }
    }

    public void EnemyKnockback()
    {
        rb.velocity = initialVelocity;
        rb.velocity = new Vector2(rb.velocity.x * PlayerCharacterScr.m_faxis + 1.5f, rb.velocity.y + 2f);
        Debug.Log("ENEMY KNCOKCBACK!");
    }

    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        gameObject.SetActive(false);
    }

    public void Flip(GameObject player)
    {
        Vector2 currentScale = player.transform.localScale;
        currentScale.x *= -1;
        player.transform.localScale = currentScale;
        enemyRight = !enemyRight;
    }


    IEnumerator C_EnemyMove()
    {
        Move();
        yield return new WaitForSeconds(0.75f);
    }

    IEnumerator C_SpriteFlash()
    {
        enemySprite.color = new Color(1f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        enemySprite.material = flashMaterial;
        yield return new WaitForSeconds(0.05f);
        enemySprite.color = defaultColor;
        enemySprite.material = defaultMaterial;
    }
}
