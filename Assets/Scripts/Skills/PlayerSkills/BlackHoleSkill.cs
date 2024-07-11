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
    [SerializeField] private float blackHoleDuration;

    private BlackHoleSkillController blackHoleController;

    public override bool CanUseSkill(){

        return base.CanUseSkill();
    }

    public override void UseSkill(){

        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        blackHoleController = newBlackHole.GetComponent<BlackHoleSkillController>();
        blackHoleController.SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttacks, cloneAttackCooldown, blackHoleDuration);
    }

    protected override void Start(){

        base.Start();
    }

    override protected void Update(){

        base.Update();
    }

    public bool ExitTrance(){

        if(!blackHoleController)
            return false;

        if (blackHoleController.playerCanExitState)
        {
            blackHoleController = null;
            return true;
        }
        return false;
    }
    public float GetBlackHoleRadius() => maxSize/2;
    

}