using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class PopUpText : MonoBehaviour
{
    public float initialVelocity = 2f;
    public float initialXVelocityRange = 3f;
    public float lifeTime = 0.8f;

    Rigidbody2D rb;
    TMP_Text damageText;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        damageText = GetComponentInChildren<TMP_Text>();
        gameObject.SetActive(false);
    }

    public void ShowText(float damage)
    {
        gameObject.SetActive(true);
        damageText.text = damage.ToString();
        StartCoroutine(C_ShowText(damageText.text));
    }

    IEnumerator C_ShowText(string text)
    {
        rb.velocity = new Vector2(Random.Range(-initialXVelocityRange, initialXVelocityRange), initialVelocity);
        yield return new WaitForSeconds(lifeTime);
        gameObject.SetActive(false);
    }  
}
