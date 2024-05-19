using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        playerRB.velocity = new Vector2(playerRB.velocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        if (playerRB.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}