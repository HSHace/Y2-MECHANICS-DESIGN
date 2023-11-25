using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerClip : MonoBehaviour
{
    [SerializeField] Transform CastPositionUpper;
    [SerializeField] Transform CastPositionLower;
    [SerializeField] LayerMask m_LayerMask;
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
        CornerClipping();

        //Debug.Log($"Upper cast: {upperCheck}");

        if (upperCheck == true && lowerCheck == false)
        {
            rb.velocity = new Vector2(rb.velocity.x + 200f, rb.velocity.y);
            //Debug.Log("X Velocity applied");
        }
        else if (upperCheck == false && lowerCheck == true)
        {
            rb.velocity = new Vector2(rb.velocity.x + 200f, rb.velocity.y + 200f);
            //Debug.Log("XY - Velocity applied");
        }
    }

    public void CornerClipping()
    {
        upperCheck = Physics2D.CircleCast(CastPositionUpper.position, CircleRadius, Vector2.zero, 0f, m_LayerMask);
        lowerCheck = Physics2D.CircleCast(CastPositionLower.position, CircleRadius, Vector2.zero, 0f, m_LayerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(CastPositionUpper.position, CircleRadius);
        Gizmos.DrawSphere(CastPositionLower.position, CircleRadius);
    }
}
