using Unity.VisualScripting;
using UnityEngine;

public class EnemySkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemySkeleton;
    private float moveDirection;
    public EnemySkeletonBattleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName)
    {
        this.enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();

        player = GameObject.Find("Player").transform;
    }

    public override void Update()
    {
        base.Update();

        if (player.position.x > enemySkeleton.transform.position.x)
            moveDirection = 1;
        else
            moveDirection = -1;
        
        enemy.SetVelocity(enemySkeleton.moveSpeed * moveDirection, enemySkeleton.rb.velocity.y);
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}