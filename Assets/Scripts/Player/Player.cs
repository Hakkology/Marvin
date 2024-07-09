using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Details")]
    public float[] attackMovement;
    public float counterAttackDuration = .2f;

    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    public bool IsBusy {get; private set;}
    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    [SerializeField] private float dashCooldown;
    private float dashUsageTimer;
    public float dashDirection {get; private set;}

    #region States
    
    public PlayerStateMachine stateMachine {get; private set;}
    public PlayerGroundedState groundedState {get; private set;}
    public PlayerIdleState idleState {get; private set;}
    public PlayerMoveState moveState{get; private set;}    
    public PlayerJumpState jumpState {get; private set;}
    public PlayerAirState airState{get; private set;}
    public PlayerWallSlideState wallSlideState{get; private set;}
    public PlayerDashState dashState{get; private set;}
    public PlayerWallJumpState wallJumpState{get; private set;}
    public PlayerPrimaryAttackState primaryAttackState{get; private set;}
    public PlayerCounterAttackState counterAttackState{get; private set;}
    #endregion

    protected override void Awake() 
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "Slide");
        dashState = new PlayerDashState (this, stateMachine, "Dash");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
    }

    protected override void Start() 
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update() 
    {
        base.Update();
        stateMachine.currentState.Update();
        CheckForDashInput();
    }

    private void CheckForDashInput(){

        if (IsWallDetected())
            return;

        dashUsageTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashUsageTimer < 0){

            dashUsageTimer = dashCooldown;
            dashDirection =  Input.GetAxisRaw("Horizontal");

            if(dashDirection == 0)
                dashDirection = facingDirection;
            
            stateMachine.ChangeState(dashState);
        }
    }

    public IEnumerator BusyFor(float seconds) {
        IsBusy = true;
        yield return new WaitForSeconds(seconds);
        IsBusy = false;
    }


    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();




}
