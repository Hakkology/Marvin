using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderEffectController : MonoBehaviour
{
    protected PlayerStats playerStats;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Enemy>() != null)
        {
            EnemyStats enemyTarget = other.GetComponent<EnemyStats>();
            if (enemyTarget != null)
                playerStats.DoMagicalDamage(enemyTarget);
        }
    }
}
