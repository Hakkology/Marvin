using UnityEngine;

public class BlackHoleSkill : Skill {

    [SerializeField] private GameObject blackHolePrefab;
    [Header("Black Hole Info")]
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [Header("Clone Info")]
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;

    public override bool CanUseSkill(){

        return base.CanUseSkill();
    }

    public override void UseSkill(){

        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab);
        BlackHoleController controller = newBlackHole.GetComponent<BlackHoleController>();
        controller.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown);
    }

    protected override void Start(){

        base.Start();
    }

    override protected void Update(){

        base.Update();
    }

}