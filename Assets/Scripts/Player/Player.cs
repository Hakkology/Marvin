using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed = 5f;

    #region Components
    public Animator playerAnim {get; private set;}
    public Rigidbody2D playerRb {get; private set;}
    #endregion

    #region States
    public PlayerStateMachine stateMachine {get; private set;}
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState{get; private set;}
    #endregion

    private void Awake() 
    {
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
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
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        playerRb.velocity = new Vector2(_xVelocity, _yVelocity);
    }
}
