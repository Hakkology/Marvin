using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    public DashSkill dashSkill {get; private set;}
    void Awake() {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }

    void Start() {
        dashSkill = GetComponent<DashSkill>();
    }
}
