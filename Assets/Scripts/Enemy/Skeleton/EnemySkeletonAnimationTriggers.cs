using UnityEngine;
public class EnemySkeletonAnimationTriggers : MonoBehaviour 
{
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            var player = hit.GetComponent<Player>();

            if(player != null)
                player.Damage();
        }
    }
} 