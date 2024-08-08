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

            if(enemy != null)
                player.stats.DoDamage(enemy.stats);
                
            EquipmentData weaponData = PlayerInventory.Instance.GetEquipment(EquipmentType.Weapon);
            if (weaponData != null)
                WeaponEffect(enemy.transform);
        }
    }

    private void WeaponEffect(Transform transform){
        EquipmentData equipmentData= PlayerInventory.Instance.GetEquipment(EquipmentType.Weapon);
        if (equipmentData!= null)
        {
            equipmentData.ExecuteItemEffect(transform);
        }

    }

    private void ThrowSword(){
        SkillManager.Instance.swordSkill.CreateSword();
    }
} 