using UnityEngine;

public class EnemySkeletonGroundedState : EnemyState
{
    protected EnemySkeleton enemySkeleton;
    protected Transform player;
    public EnemySkeletonGroundedState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();
        
        if (enemySkeleton.IsPlayerDetected() || Vector2.Distance(enemySkeleton.transform.position, player.transform.position) < 2)
            stateMachine.ChangeState(enemySkeleton.battleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}