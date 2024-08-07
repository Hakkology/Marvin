using System.Collections;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack Details")]
    public float[] attackMovement;
    public float counterAttackDuration = .2f;
    public float swordReturnImpact;

    [Header("Move Info")]
    public float moveSpeed;
    public float jumpForce;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    public bool IsBusy {get; private set;}
    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDirection {get; private set;}
    private float defaultDashSpeed;

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
    public PlayerAimSwordState aimSwordState {get; private set;}
    public PlayerCatchSwordState catchSwordState {get; private set;}
    public PlayerBlackHoleState blackHoleState {get; private set;}
    public PlayerDeadState deadState{get; private set;}
    #endregion

    #region Managers
    public SkillManager skill {get; private set;}
    public GameObject sword {get; private set;}
    //public PlayerStats stats {get; private set;}
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
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");
        deadState = new PlayerDeadState(this, stateMachine, "Death");
    }

    protected override void Start() 
    {
        base.Start();

        //stats = GetComponent<PlayerStats>();
        skill = SkillManager.Instance;
        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }

    protected override void Update() 
    {
        base.Update();
        stateMachine.currentState.Update();
        
        CheckForDashInput();

        if(Input.GetKeyDown(KeyCode.F))
            skill.crystalSkill.CanUseSkill();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed *= 1-_slowPercentage;
        jumpForce *= 1-_slowPercentage;
        dashSpeed *= 1-_slowPercentage;
        anim.speed *= 1-_slowPercentage;

        Invoke(nameof(DefaultSpeed), _slowDuration);
    }

    public override void DefaultSpeed()
    {
        base.DefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    private void CheckForDashInput(){

        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.dashSkill.CanUseSkill()){

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

    public void AssignNewSword(GameObject _newSword) {
        sword = _newSword;
    }
    public void CatchTheSword(){
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public override void Death()
    {
        base.Death();
        stateMachine.ChangeState(deadState);
    }
}
