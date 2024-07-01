using UnityEngine;

public class Entity : MonoBehaviour {

    #region Info variables
    [Header("Collision Info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance = 8f;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [Header("Attack Info")]
    public float attackCheckRadius;
    public Transform attackCheck;
    #endregion


    #region Components
    public Animator anim {get; private set;}
    public Rigidbody2D rb {get; private set;}
    #endregion

    public int facingDirection {get; private set;} = 1;
    protected bool facingRight = true;
    
    protected virtual void Awake (){

    }

    protected virtual void Start(){

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update(){

    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    
    public void ZeroVelocity() => rb.velocity = new Vector2(0,0);
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);

    protected virtual void OnDrawGizmos() 
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*facingDirection, wallCheck.position.y));
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
    protected virtual void FlipController(float _x){
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