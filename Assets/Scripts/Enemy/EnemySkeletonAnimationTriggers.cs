using UnityEngine;
public class EnemySkeletonAnimationTriggers : MonoBehaviour 
{
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationTrigger();
    }
} 