public class EnemySkeleton : Enemy 
{
    public EnemySkeletonIdleState idleState{get; private set;}
    public EnemySkeletonMoveState moveState{get; private set;}
    public EnemySkeletonMoveState battleState{get; private set;}
    protected override void Awake() 
    {
        base.Awake();

        idleState = new EnemySkeletonIdleState(this, stateMachine, "Idle", this);
        moveState = new EnemySkeletonMoveState(this, stateMachine, "Move", this);
        battleState = new EnemySkeletonMoveState(this, stateMachine, "Move", this);
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