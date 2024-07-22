using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CrystalSkill : Skill {

    [SerializeField] private GameObject crystalPrefab;
    [Header("Crystal Values")]
    [SerializeField] private float crystalDuration;

    [Header("Explosive Crystal")]
    [SerializeField] private float crystalGrowSpeed;
    [SerializeField] private float crystalMaxSize;

    [Header("Moving Crystal")]
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private int amountOfStacks;
    [SerializeField] private int multiStackCooldown;
    [SerializeField] private float multiStackUseTimeWindow;
    [SerializeField] private List<GameObject> crystals = new List<GameObject>();

    [Header("Various Crystal Skill Modifiers")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private bool canExplode;
    [SerializeField] private bool cloneInsteadOfCrystal;

    private GameObject currentCrystal;
    protected override void Start() {
        base.Start();
    }

    protected override void Update(){
        base.Update();
    }

    public override void UseSkill(){
        base.UseSkill();

        if(CanUseMultiCrystals())
            return;

        if(currentCrystal == null)
            CreateCrystal();
        else
        {
            if(canMoveToEnemy)
                return;
            
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if (cloneInsteadOfCrystal){
                player.skill.cloneSkill.CreateClone(currentCrystal.transform, Vector2.zero);
                Destroy(currentCrystal);
            } else 
                currentCrystal.GetComponent<CrystalSkillController>()?.CrystalRelease();
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        CrystalSkillController crystalSkillController = currentCrystal.GetComponent<CrystalSkillController>();
        crystalSkillController.SetupCrystal(
            crystalDuration, canExplode, canMoveToEnemy, moveSpeed, crystalGrowSpeed, crystalMaxSize, FindClosestEnemy(currentCrystal.transform), player);
    }
    public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<CrystalSkillController>().ChooseRandomEnemy();
    private bool CanUseMultiCrystals(){
        if (canUseMultiStacks){

            if (crystals.Count > 0)
            {
                if(crystals.Count == amountOfStacks)
                    Invoke("ResetAbility", multiStackUseTimeWindow);

                cooldown = 0;
                GameObject crystalToSpawn =  crystals[crystals.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystals.Remove(crystalToSpawn);

                newCrystal.GetComponent<CrystalSkillController>().SetupCrystal(
                    crystalDuration, canExplode, canMoveToEnemy, moveSpeed, crystalGrowSpeed, crystalMaxSize, FindClosestEnemy(newCrystal.transform), player);
                
                if (crystals.Count <= 0){
                    cooldown = multiStackCooldown;
                    RefillCrystal();
                }
                return true;
            }
        }
        return false;
    }

    private void RefillCrystal(){
        int amountToAdd = amountOfStacks - crystals.Count;

        for (int i = 0; i < amountToAdd; i++)
            crystals.Add(crystalPrefab);
    }

    private void ResetAbility(){
        if (cooldownTimer > 0)
            return;
        
        cooldownTimer = multiStackCooldown;
        RefillCrystal();
    }

    public override bool CanUseSkill(){
        return base.CanUseSkill();
    }
}
