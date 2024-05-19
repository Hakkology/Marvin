using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviourOld : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float jumpForce;
    [Header("Collision Checks")]
    [SerializeField]
    private float groundCheckDistance;
    [SerializeField]
    private LayerMask groundLayer;

    [Header("Dash Values")]
    [SerializeField]
    private float dashDuration;
    [SerializeField]
    private float dashSpeed;
    [SerializeField]
    private float dashCooldown;
    private float dashCooldownTimer;
    private float dashTime;

    // INTERNAL VALUES

    // for lateral movement
    private float xInput;
    // for flipping
    private int facingDirection = 1;
    private bool facingRight = true;
    // grounding
    private bool isGrounded;
    // dash


    // REFERENCES
    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        MovementController();
        FlipController();
        DashController();
        InputController();
        AnimatorControllers();
        CollisionChecks();
    }

    private void DashController()
    {
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
    }

    private void InputController()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
        if (Input.GetKeyDown(KeyCode.LeftShift)) DashAbility();
    }

    private void DashAbility()
    {
        if (dashCooldownTimer <0)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }
    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundLayer);
    }

    private void MovementController()
    {
        if (dashTime > 0)
        {
            rb.velocity = new Vector2(xInput * dashSpeed, 0);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
        
    }
    private void Jump()
    {
        if (isGrounded) rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimatorControllers()
    {
        bool isMoving = rb.velocity.x !=0;
        anim.SetBool("IsMoving", isMoving);
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("IsDashing", dashTime > 0);
    }

    private void Flip()
    {
        facingDirection = facingDirection *-1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }

    void OnDrawGizmos()
    {
        // from - to
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
