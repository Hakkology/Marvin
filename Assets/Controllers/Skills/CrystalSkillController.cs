
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;
    private Transform closestTarget;
    private Player player;
    [SerializeField] LayerMask whatIsEnemy;
    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;
    private bool canGrow;
    private float crystalGrowSpeed;
    private float crystalMaxSize;
    
    public void SetupCrystal(
        float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, float _growSpeed, float _crystalMaxSize, Transform _closestTarget, Player _player){
        player = _player;
        crystalExistTimer = _crystalDuration;
        moveSpeed = _moveSpeed;
        canExplode = _canExplode;
        canMove = _canMove;
        crystalGrowSpeed = _growSpeed;
        crystalMaxSize = _crystalMaxSize;
        closestTarget = _closestTarget;
    }
    void Awake() {
        anim = GetComponent<Animator>();
        cd = GetComponent<CircleCollider2D>();
    }
    void Update() {

        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
            CrystalRelease();
        
        if(canMove && closestTarget != null) {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, closestTarget.position) < .5f){
                canMove = false;
                CrystalRelease();
            }
        }
        
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(crystalMaxSize, crystalMaxSize), crystalGrowSpeed * Time.deltaTime);
        
    }
    public void CrystalRelease()
    {
        if (canExplode){
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    private void AnimationExplodeEvent(){

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            var enemy = hit.GetComponent<Enemy>();

            if(enemy != null)
                player.stats.DoMagicalDamage(enemy.GetComponent<EntityStats>());
        }
    }
    public void SelfDestroy() => Destroy(gameObject);
    public void ChooseRandomEnemy(){

        float radius = SkillManager.Instance.blackHoleSkill.GetBlackHoleRadius();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius , whatIsEnemy);

        if(colliders.Length > 0)
            closestTarget = colliders[Random.Range(0, colliders.Length)].transform;
    }
}
