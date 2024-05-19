using UnityEngine;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected Rigidbody2D playerRB;
    protected float xInput;
    protected float yInput;
    protected float stateTimer;


    private string animBoolName;

    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName){
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        playerRB = player.playerRb;
        player.playerAnim.SetBool(animBoolName, true);
    }
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.playerAnim.SetFloat("yVelocity", playerRB.velocity.y);
    }

    public virtual void Exit()
    {
        player.playerAnim.SetBool(animBoolName, false);
    }


}