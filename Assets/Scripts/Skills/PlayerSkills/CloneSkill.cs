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
    [SerializeField] private bool canAttack;
    [SerializeField] private float chanceToDuplicate;

    [Header("Various Clone Skill modifiers")]
    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;
    [SerializeField] private bool createDuplicateClone;
    

    public void CreateClone(Transform clonePosition, Vector2 _offset){

        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(
            clonePosition, cloneDuration, colourLosingSpeed, canAttack, _offset, FindClosestEnemy(newClone.transform), createDuplicateClone, chanceToDuplicate);
    }

    public void CreateCloneOnDashStart(){
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver(){
        if (createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform){
        if (createCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, .2f, new Vector2(1.5f * player.facingDirection, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _enemyTransform, float _delayTime, Vector2 _offset){
        yield return new WaitForSeconds(_delayTime);
        CreateClone(_enemyTransform, _offset);
    }
}
