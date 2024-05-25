public class EnemySkeletonBattleState : EnemySkeletonGroundedState
{
    public EnemySkeletonBattleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName, _enemySkeleton)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}