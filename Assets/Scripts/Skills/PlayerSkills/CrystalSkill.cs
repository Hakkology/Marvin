using UnityEngine;

public class CrystalSkill : Skill {

    [SerializeField] private GameObject crystal;
    protected override void Start() {
        base.Start();
    }

    protected override void Update(){
        base.Update();
    }

    public override bool CanUseSkill(){
        return base.CanUseSkill();
    }
}
