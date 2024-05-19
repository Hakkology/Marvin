
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (xInput != 0 && player.facingDirection != xInput)
            stateMachine.ChangeState(player.airState);

        if (yInput < 0)
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        
        else
            playerRB.velocity = new Vector2(0, playerRB.velocity.y * .4f);
        
        if(player.IsGroundDetected() || !player.IsWallDetected())
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}