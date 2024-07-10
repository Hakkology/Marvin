using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.R) && !player.skill.blackHoleSkill.IsInCooldown())
            stateMachine.ChangeState(player.blackHoleState);

        if(Input.GetKeyDown(KeyCode.Q))
            stateMachine.ChangeState(player.counterAttackState);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);
        
        // is ground detected eklemezsek bu sefer düşmanların üstünden atlayabilir.
        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            stateMachine.ChangeState(player.primaryAttackState);
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimSwordState);

        // if (Input.GetKeyDown(KeyCode.LeftShift))
        //     stateMachine.ChangeState(player.dashState);
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool HasNoSword(){
        if (!player.sword)
            return true;
        
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }

}