using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public DashSkill dashSkill {get; private set;}
    public CloneSkill cloneSkill{get; private set;}
    public SwordSkill swordSkill{get; private set;}
    public BlackHoleSkill blackHoleSkill{get; private set;}
    public CrystalSkill crystalSkill{get; private set;}
    void Awake() {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    void Start() {
        dashSkill = GetComponent<DashSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        swordSkill = GetComponent<SwordSkill>();
        blackHoleSkill = GetComponent<BlackHoleSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
    }
}
