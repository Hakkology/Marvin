using UnityEngine;

public class CloneSkillController : MonoBehaviour {
    
    private SpriteRenderer sRenderer;
    private Animator anim;
    private Player player;
    private float colourLosingSpeed;
    private float cloneTimer;
    private Transform closestEnemy;
    private bool createDuplicateClone;
    private float chanceToDuplicate = 20;
    private int facingDirection = 1;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    void Awake() {
        player = PlayerManager.Instance.player;
        sRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update() {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sRenderer.color = new Color(1,1,1, sRenderer.color.a - (Time.deltaTime * colourLosingSpeed));
            if (sRenderer.color.a <= 0)
                Destroy(gameObject);
        }
    }
    public void SetupClone(
        Transform _newTransform, float _cloneDuration, float _colorLosingSpeed, 
        bool canAttack, Vector3 _offset, Transform _closestEnemy, bool _createDuplicateClone, float _chanceToDuplicate) {

        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        colourLosingSpeed = _colorLosingSpeed;
        closestEnemy = _closestEnemy;
        createDuplicateClone = _createDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;

        FaceClosestTarget();
    }

    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);

        foreach (var hit in colliders)
        {
            var enemy = hit.GetComponent<Enemy>();

            if(enemy != null)
            {
                enemy.DamageEffect();

                if (createDuplicateClone){
                    if (Random.Range(0, 100) < chanceToDuplicate){
                        player.skill.cloneSkill.CreateClone(hit.transform, new Vector2(.5f * facingDirection, 0));
                    }
                }
            }
        }
    }

    private void FaceClosestTarget(){

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x){
                facingDirection = -1;
                transform.Rotate(0, 180, 0);
            }
        }
    }
}