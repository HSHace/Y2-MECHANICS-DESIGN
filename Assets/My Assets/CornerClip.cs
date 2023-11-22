using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerClip : MonoBehaviour
{
    [SerializeField] BoxCollider2D UpperCornerClip;
    [SerializeField] BoxCollider2D LowerCornerClip;
    [SerializeField] Transform CastPositionUpper;
    [SerializeField] Transform CastPositionLower;
    [SerializeField] private float CircleRadius;

    private bool upperCheck;
    private bool lowerCheck;

    Rigidbody2D rb;
    PlayerCharacter PlayerCharacterScr;

    private void Awake()
    {
        PlayerCharacterScr = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        if(upperCheck == true && lowerCheck == false && PlayerCharacterScr.isJumping == true)
        {
            rb.velocity = new Vector2 (rb.velocity.x + 2f, rb.velocity.y);
            Debug.Log("UPPER CORNER CLIP VELOCITY");
        }
        else if(upperCheck == false && lowerCheck == true && PlayerCharacterScr.isJumping == true)
        {
            rb.velocity = new Vector2(rb.velocity.x + 2f, rb.velocity.y + 2f);
        }
    }

    public void CornerClipping()
    {
        upperCheck = Physics2D.CircleCast(CastPositionUpper.position, CircleRadius, Vector2.zero, 0, PlayerCharacterScr.m_LayerMask);
        lowerCheck = Physics2D.CircleCast(CastPositionLower.position, CircleRadius, Vector2.zero, 0, PlayerCharacterScr.m_LayerMask);
    }
}
