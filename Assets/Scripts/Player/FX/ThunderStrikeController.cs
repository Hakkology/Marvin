using UnityEngine;

public class ThunderStrikeController : MonoBehaviour {
    
    [SerializeField] private EntityStats targetStats;
    [SerializeField] private float targetSpeed;

    private Animator anim;
    private int damage;
    private bool triggered = false;

    void Awake() {
        anim = GetComponentInChildren<Animator>();
    }

    public void SetupThunderStrike(int _damage, EntityStats _targetStats) {
        damage = _damage;
        targetStats = _targetStats;
    }
    void Update() {

        if (!targetStats) return;
        if (triggered) return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, targetSpeed * Time.deltaTime);
        transform.right = transform.position - targetStats.transform.position;

        if (Vector2.Distance(transform.position, targetStats.transform.position)< .3f && !triggered)
        {

            //anim.transform.localRotation = Quaternion.identity;
            transform.position += new Vector3(0, 2.75f);
            transform.localScale = new Vector3(3, 3, 1);
            anim.transform.localRotation = Quaternion.identity;
            transform.localRotation = Quaternion.identity;

            triggered = true;
            anim.SetTrigger("Hit");

            Invoke(nameof(DamageAndSelfDestroy), .2f);
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock();
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }
}