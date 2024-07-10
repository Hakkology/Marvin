using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime = .5f;
    private bool skillUsed;
    private float defaultGravity;
    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) 
    : base(_player, _stateMachine, _animBoolName)
    {

    }
    public override void Enter(){

        base.Enter();

        defaultGravity = player.rb.gravityScale;

        skillUsed = false;
        stateTimer = flyTime;
        player.rb.gravityScale = 0;
    }

    public override void Update(){
        base.Update();

        if (stateTimer > 0)
            player.rb.velocity = new Vector2(0,8);
        
        if (stateTimer < 0){

            player.rb.velocity = new Vector2(0, -.1f);

            if(!skillUsed){

                if(player.skill.blackHoleSkill.CanUseSkill())
                    skillUsed = true;
            }
        }

        if (player.skill.blackHoleSkill.ExitTrance())
            stateMachine.ChangeState(player.airState);
    }

    public override void Exit(){

        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.fx.TurnInvisible(false);
    }

    public override void AnimationFinishTrigger(){
        base.AnimationFinishTrigger();
    }
}