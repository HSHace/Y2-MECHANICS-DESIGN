using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    [SerializeField] public float currentEnemyHealth;
    [SerializeField] public float maxEnemyHealth = 100.0f;
    [SerializeField] SpriteRenderer enemySprite;
    [SerializeField] Material flashMaterial;

    Material defaultMaterial;
    Color defaultColor;

    private bool enemyDead;

    private void Awake()
    {
        enemySprite = GetComponent<SpriteRenderer>();
        currentEnemyHealth = maxEnemyHealth;
        defaultColor = enemySprite.color;
        defaultMaterial = enemySprite.material;
    }

    public void TakeDamage(float damage)
    {
        currentEnemyHealth -= damage;
        StartCoroutine(C_SpriteFlash());

        if(currentEnemyHealth <= 0)
        {
            enemyDead = true;
            Die();
        }
    }

    public void Die()
    {
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        gameObject.SetActive(false);
        enemyDead = true;
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
