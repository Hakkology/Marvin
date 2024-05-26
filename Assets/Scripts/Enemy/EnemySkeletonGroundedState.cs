using UnityEngine;

public class EnemySkeletonGroundedState : EnemyState
{
    protected EnemySkeleton enemySkeleton;
    public EnemySkeletonGroundedState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        if (enemySkeleton.IsPlayerDetected())
            stateMachine.ChangeState(enemySkeleton.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}