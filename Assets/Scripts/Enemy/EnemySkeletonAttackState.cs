using Unity.VisualScripting;
using UnityEngine;

public class EnemySkeletonAttackState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemySkeleton;
    public EnemySkeletonAttackState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        enemy.ZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(enemySkeleton.battleState);
        
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttacked = Time.time;
    }
}