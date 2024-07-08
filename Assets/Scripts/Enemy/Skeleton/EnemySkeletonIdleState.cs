public class EnemySkeletonIdleState : EnemySkeletonGroundedState
{
    public EnemySkeletonIdleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName, _enemySkeleton)
    {
    
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
        
        // if (enemy.IsPlayerDetected())
        //     stateMachine.ChangeState(enemySkeleton.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}