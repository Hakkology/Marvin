using UnityEngine;

public class CloneSkillController : MonoBehaviour {
    
    private SpriteRenderer sRenderer;
    private Animator anim;
    private float colourLosingSpeed;
    private float cloneTimer;
    private Transform closestEnemy;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;

    void Awake() {
        sRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update() {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sRenderer.color = new Color(1,1,1, sRenderer.color.a - (Time.deltaTime * colourLosingSpeed));
            if (sRenderer.color.a < 0)
                Destroy(gameObject);
        }
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, float _colorLosingSpeed, bool canAttack, Vector3 _offset, Transform _closestEnemy) {

        if (canAttack)
            anim.SetInteger("AttackNumber", Random.Range(1, 4));
        
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;
        colourLosingSpeed = _colorLosingSpeed;
        closestEnemy = _closestEnemy;

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
                enemy.Damage();
        }
    }

    private void FaceClosestTarget(){

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}