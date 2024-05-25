public class EnemySkeletonMoveState : EnemySkeletonGroundedState
{
    public EnemySkeletonMoveState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName, _enemySkeleton)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemySkeleton.SetVelocity(enemySkeleton.moveSpeed * enemySkeleton.facingDirection, enemySkeleton.rb.velocity.y);

        if (enemySkeleton.IsWallDetected() || !enemySkeleton.IsGroundDetected())
        {
            enemySkeleton.Flip();
            stateMachine.ChangeState(enemySkeleton.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}