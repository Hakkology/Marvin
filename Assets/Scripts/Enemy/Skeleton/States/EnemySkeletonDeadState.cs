using UnityEngine;

public class EnemySkeletonDeadState : EnemyState
{
    private EnemySkeleton enemySkeleton;
    public EnemySkeletonDeadState(Enemy _enemy, EnemyStateMachine _stateMachine, string _animBoolName, EnemySkeleton _enemySkeleton) 
    : base(_enemy, _stateMachine, _animBoolName)
    {
        enemySkeleton = _enemySkeleton;
    }
    public override void Enter()
    {
        base.Enter();
        enemySkeleton.anim.SetBool(enemy.lastAnimBoolName, true);
        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .15f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            enemy.rb.velocity = new Vector2(0, 10);
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}