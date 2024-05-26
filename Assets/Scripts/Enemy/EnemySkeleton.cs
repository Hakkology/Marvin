public class EnemySkeleton : Enemy 
{
    public EnemySkeletonIdleState idleState{get; private set;}
    public EnemySkeletonMoveState moveState{get; private set;}
    public EnemySkeletonBattleState battleState{get; private set;}
    public EnemySkeletonAttackState attackState{get; private set;}
    protected override void Awake() 
    {
        base.Awake();

        idleState = new EnemySkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new EnemySkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new EnemySkeletonBattleState(this, stateMachine, "Move", this);
        attackState = new EnemySkeletonAttackState(this, stateMachine, "Attack", this);
    }

    protected override void Start() 
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update() 
    {
        base.Update();
    }
}