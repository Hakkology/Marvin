using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashDirection {get; private set;}
    public int facingDirection {get; private set;} = 1;
    private bool facingRight = true;
    

    [Header("Collision Info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 8f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;

    #region Components
    public Animator playerAnim {get; private set;}
    public Rigidbody2D playerRb {get; private set;}
    #endregion

    #region States
    
    public PlayerStateMachine stateMachine {get; private set;}
    public PlayerGroundedState groundedState {get; private set;}
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState{get; private set;}    
    public PlayerJumpState jumpState {get; private set;}
    public PlayerAirState airState{get; private set;}
    public PlayerWallSlideState wallSlideState{get; private set;}
    public PlayerDashState dashState{get; private set;}
    #endregion

    private void Awake() 
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "Slide");
        dashState = new PlayerDashState (this, stateMachine, "Dash");

    }

    private void Start() 
    {
        playerAnim = GetComponentInChildren<Animator>();
        playerRb = GetComponent<Rigidbody2D>();

        stateMachine.Initialize(idleState);
    }

    private void Update() 
    {
        
        stateMachine.currentState.Update();
        CheckForDashInput();
    }

    private void CheckForDashInput(){

        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0){

            dashUsageTimer = dashCooldown;
            dashDirection =  Input.GetAxisRaw("Horizontal");

            if(dashDirection == 0)
                dashDirection = facingDirection;
            
            stateMachine.ChangeState(dashState);
        }
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        playerRb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);

    private void OnDrawGizmos() 
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*facingDirection, wallCheck.position.y));
    }

    public void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    private void FlipController(float _x){
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
}
