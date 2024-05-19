public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        if (stateTimer<0)
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.dashSpeed * player.dashDirection, 0);

        base.Update();
    }

    public override void Exit()
    {
        player.SetVelocity(0, playerRB.velocity.y);

        base.Exit();
    }
}