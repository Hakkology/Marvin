using UnityEngine;

public class Enemy : Entity 
{
    [SerializeField]
    protected LayerMask PlayerLayer;
    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public EnemyStateMachine stateMachine {get; private set;} 

    protected override void Awake() 
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();    
    }

    protected override void Start() 
    {
        base.Start();
        
    }

    protected override void Update() 
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 50, PlayerLayer);
}