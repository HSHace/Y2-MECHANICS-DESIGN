using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rasengan : MonoBehaviour
{
    [SerializeField] float rasenganDamage = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyCharacter enemyCharacter = collision.gameObject.GetComponent<EnemyCharacter>();
            enemyCharacter.transform.position = transform.position;
            enemyCharacter.TakeDamage(rasenganDamage, 1f, 2f);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyCharacter enemyCharacter = collision.gameObject.GetComponent<EnemyCharacter>();
            Vector2 contactLocation = new Vector2(transform.position.x + 0.4f, transform.position.y);

            if(enemyCharacter.currentEnemyHealth > 1)
            {
                enemyCharacter.transform.position = contactLocation;
                enemyCharacter.TakeDamage(rasenganDamage, 0.1f, 0f);
            }
            else if (enemyCharacter.currentEnemyHealth < 1)
            {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(transform.right * 40, ForceMode2D.Impulse);
            }
        }
    }
}
