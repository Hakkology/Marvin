using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/ItemEffect/Ice And Fire", order = 1)]
public class IceAndFireEffect : ItemEffect
{
    [SerializeField] private GameObject iceAndFirePrefab;
    [SerializeField] private float xVelocity;
    public override void ExecuteEffect(Transform transform)
    {
        Player player = PlayerManager.Instance.player;
        bool thirdAttack = player.primaryAttackState.comboCounter == 2;
        if (thirdAttack)
        {
            GameObject newIceAndFire = Instantiate(iceAndFirePrefab, transform.position, player.transform.rotation);
            newIceAndFire.GetComponent<Rigidbody2D>().velocity = new Vector2(xVelocity * player.facingDirection, 0);
            Destroy(newIceAndFire, 4);
        }
    }
}
