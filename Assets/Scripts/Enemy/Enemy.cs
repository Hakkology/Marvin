using System.Collections;
using UnityEngine;

public class Enemy : Entity 
{
    [SerializeField]
    protected LayerMask PlayerLayer;

    [Header("Move Info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("Attack Info")]
    public float attackDistance;
    public float attackCooldown;

    [Header("Stunned Info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;
    
    [HideInInspector] public float lastTimeAttacked;

    public EnemyStateMachine stateMachine {get; private set;} 
    public string lastAnimBoolName {get; private set;}
    //public EnemyStats stats {get; private set;}


    protected override void Awake() 
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;    
    }

    protected override void Start() 
    {
        base.Start();
        //stats = GetComponent<EnemyStats>();
    }

    protected override void Update() 
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 50, PlayerLayer);
    public virtual void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public virtual void AssignLastAnimName(string _animBoolName){
        lastAnimBoolName = _animBoolName;
    }
    protected override void OnDrawGizmos() 
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + attackDistance * facingDirection, transform.position.y));
    }

    #region FreezeTime
    public virtual void FreezeTime(bool _timeFrozen){
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimeFor (float _seconds){
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    #endregion

    #region CounterAttackWindow
    public virtual void OpenCounterAttackWindow(){
        canBeStunned = true;
        counterImage.SetActive(true);
    }
    public virtual void CloseCounterAttackWindow(){
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion
    
    #region Stun
    protected virtual bool CanBeStunned(){
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public bool CheckStun(){
        return CanBeStunned();
    }
    #endregion
}