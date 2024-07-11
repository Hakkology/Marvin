using UnityEngine;
public class PlayerAnimationTriggers : MonoBehaviour 
{
    private Player player => GetComponentInParent<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            var enemy = hit.GetComponent<Enemy>();

            if(enemy != null){
                enemy.Damage();
                enemy.stats.TakeDamage(player.stats.damage.GetValue());
            }
        }
    }

    private void ThrowSword(){
        SkillManager.Instance.swordSkill.CreateSword();
    }
} 