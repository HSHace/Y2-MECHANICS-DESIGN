using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] float jumpForce;
    [SerializeField] float slamForce;
    [SerializeField] float moveSpeed;
    [SerializeField] float circleRadius;

    [SerializeField] Transform castPosition;

    [SerializeField] LayerMask m_LayerMask;

    bool isJumping;
    bool isGrounded;
    float m_f_axis;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = Physics2D.CircleCast(castPosition.position, circleRadius, Vector2.zero, 0, m_LayerMask);
        rb.velocity = new Vector2(m_f_axis * moveSpeed, rb.velocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        m_f_axis = context.ReadValue<float>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    public void Slam(InputAction.CallbackContext context)
    {
        if(isJumping)
        {
            rb.AddForce(Vector2.down * slamForce, ForceMode2D.Impulse);
        }

        isJumping = false;
    }

    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(castPosition.position, circleRadius);
        }
        else
        {
            Gizmos.color= Color.red;
            Gizmos.DrawSphere(castPosition.position, circleRadius);
        }
    }
}
