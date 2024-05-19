public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        if (xInput != 0)
            stateMachine.ChangeState(player.moveState);

        base.Update();

    }

    public override void Exit()
    {
        base.Exit();
    }
}