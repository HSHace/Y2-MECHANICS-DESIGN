using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public float ShurikenDamage;

    Rigidbody2D rb;
    PlayerCharacter PlayerCharacterScr;
    InputHandler InputHandlerScr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        PlayerCharacterScr = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();
        InputHandlerScr = GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyCharacter enemyCharacter = collision.gameObject.GetComponent<EnemyCharacter>();
            enemyCharacter.TakeDamage(ShurikenDamage);
            GameObject enemy = collision.gameObject;
            enemy.GetComponent<PopUpText>();

            Destroy(gameObject);
        }

        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        if(gameObject!= null)
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}
