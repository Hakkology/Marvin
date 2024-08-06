public class PlayerStats : EntityStats
{
    private Player player;
    private PlayerItemDrop itemDrop;
    protected override void Start()
    {
        base.Start();
        player = GetComponent<Player>();
        itemDrop = GetComponent<PlayerItemDrop>();
    }

    public override void TakeDamage(int _damage)
    {
        //base.TakeDamage(_damage);
        player.DamageEffect();
    }

    protected override void Die()
    {
        base.Die();
        player.Death();
        itemDrop.GenerateDrop();
    }
}