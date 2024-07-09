using Unity.VisualScripting;
using UnityEngine;

public class EnemySkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemySkeleton;
    private float moveDirection;
    public EnemySkeletonBattleState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) : base(_enemy, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.Instance.player.transform;
    }

    public override void Update()
    {
        base.Update();

        if (enemySkeleton.IsPlayerDetected().collider != null && enemySkeleton.IsPlayerDetected().distance < enemySkeleton.attackDistance)
        {
            stateTimer = enemySkeleton.battleTime;

            if (CanAttack())
                stateMachine.ChangeState(enemySkeleton.attackState);
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10)
            {
                stateMachine.ChangeState(enemySkeleton.idleState);
            }
        }

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

    private bool CanAttack(){

        if (Time.time >= enemySkeleton.lastTimeAttacked + enemySkeleton.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}