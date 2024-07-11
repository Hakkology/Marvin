using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CrystalSkill : Skill {

    [SerializeField] private GameObject crystalPrefab;
    [Header("Crystal Values")]
    [SerializeField] private float crystalDuration;
    [SerializeField] private float crystalGrowSpeed;
    [SerializeField] private float crystalMaxSize;

    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private int multiStackCooldown;
    [SerializeField] private float multiStackUseTimeWindow;
    [SerializeField] private List<GameObject> crystals = new List<GameObject>();

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

        if(currentCrystal == null){
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            CrystalSkillController crystalSkillController = currentCrystal.GetComponent<CrystalSkillController>();
            crystalSkillController.SetupCrystal(
                crystalDuration, canExplode, canMoveToEnemy, moveSpeed, crystalGrowSpeed, crystalMaxSize, FindClosestEnemy(currentCrystal.transform));
        }
        else{
            if(canMoveToEnemy)
                return;
            
            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            currentCrystal.GetComponent<CrystalSkillController>()?.CrystalRelease();
        }
    }

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
                    crystalDuration, canExplode, canMoveToEnemy, moveSpeed, crystalGrowSpeed, crystalMaxSize, FindClosestEnemy(newCrystal.transform));
                
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
