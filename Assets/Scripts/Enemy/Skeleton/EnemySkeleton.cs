using UnityEngine;

public class EnemySkeleton : Enemy 
{
    public EnemySkeletonIdleState idleState{get; private set;}
    public EnemySkeletonMoveState moveState{get; private set;}
    public EnemySkeletonBattleState battleState{get; private set;}
    public EnemySkeletonAttackState attackState{get; private set;}
    public EnemySkeletonStunnedState stunnedState{get; private set;}
    public EnemySkeletonDeadState deadState{get; private set;}
    protected override void Awake() 
    {
        base.Awake();

        idleState = new EnemySkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new EnemySkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new EnemySkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new EnemySkeletonAttackState(this, stateMachine, "Attack", this);
        stunnedState = new EnemySkeletonStunnedState(this, stateMachine, "Stunned", this);
        deadState = new EnemySkeletonDeadState(this, stateMachine, "Idle", this);
    }

    protected override void Start() 
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update() 
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.U))
        {
            stateMachine.ChangeState(this.stunnedState);
        }
    }

    protected override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }

        return false;
    }

    public override void Death()
    {
        base.Death();
        stateMachine.ChangeState(deadState);
    }

}