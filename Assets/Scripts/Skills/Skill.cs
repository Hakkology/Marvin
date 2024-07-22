using UnityEngine;

public class Skill : MonoBehaviour {
    
    [SerializeField] protected float cooldown;
    protected float cooldownTimer;
    protected Player player;

    protected virtual void Start() {
        player = PlayerManager.Instance.player;
    }

    protected virtual void Update(){
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill(){
        if (cooldownTimer <0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }

        return false;
    }

    public virtual void UseSkill(){
        // for skills
    }

    public virtual bool IsInCooldown(){
        if (cooldownTimer <0)
            return false;
        else
            return true;
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform){

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_checkTransform.position, 25);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null)
            {
                float distanceToEnemy = Vector2.Distance(_checkTransform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance){
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemy.transform;
                }
            }
        }

        return closestEnemy;
    }
}