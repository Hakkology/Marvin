public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if(xInput == 0)
            stateMachine.ChangeState(player.idleState);

        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, playerRB.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}