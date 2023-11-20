using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
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
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
