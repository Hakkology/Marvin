using System.Collections;
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

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;

    #endregion


    #region Components
    public Animator anim {get; private set;}
    public Rigidbody2D rb {get; private set;}
    public EntityFX fx {get; private set;}
    #endregion

    public int facingDirection {get; private set;} = 1;
    protected bool facingRight = true;
    
    protected virtual void Awake (){

    }

    protected virtual void Start(){

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update(){

    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public void Damage()
    {
        Debug.Log(this.gameObject.name + "was damaged.");
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback");
    }

    protected virtual IEnumerator HitKnockback(){
        isKnocked = true;
        rb.velocity = new Vector2(knockbackDirection.x * -facingDirection, knockbackDirection.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
    }
    public void ZeroVelocity() {
        if (isKnocked)
            return;
        
        rb.velocity = Vector2.zero;
    } 
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, groundLayer);

    protected virtual void OnDrawGizmos() 
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*facingDirection, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
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