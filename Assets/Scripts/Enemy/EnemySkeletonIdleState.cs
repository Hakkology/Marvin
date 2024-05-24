public class EnemySkeletonIdleState : EnemyState
{
    EnemySkeleton enemySkeleton;
    public EnemySkeletonIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemySkeleton, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemySkeleton.moveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}