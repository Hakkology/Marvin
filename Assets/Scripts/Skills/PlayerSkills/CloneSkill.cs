using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CloneSkill : Skill
{
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private float colourLosingSpeed;
    [Space]
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform clonePosition){

        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(clonePosition, cloneDuration, colourLosingSpeed, canAttack);
    }
}
