public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);

        if(player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);

        if (xInput != 0) 
            player.SetVelocity(player.moveSpeed * .8f * xInput, playerRB.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();


    }
}