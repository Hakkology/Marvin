using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{

    private int comboCounter;
    private float lastTimeAttacked;
    private float comboWindow = 2; // combo reset timer

    public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.playerAnim.SetInteger("ComboCounter", comboCounter);
        player.playerAnim.speed = 1.2f;

        float attackDirection = player.facingDirection;
        if (xInput != 0)
            attackDirection = xInput;
        
        // change velocity based on combo counter.
        player.SetVelocity(player.attackMovement[comboCounter]* attackDirection, playerRB.velocity.y);

        stateTimer = .1f;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.ZeroVelocity();
        
        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
        player.playerAnim.speed = 1;
        comboCounter++;
        lastTimeAttacked = Time.time;
    }
}