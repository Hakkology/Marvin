using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator anim;
    private CircleCollider2D cd;
    private Transform closestTarget;
    private float crystalExistTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;
    private bool canGrow;
    private float crystalGrowSpeed;
    private float crystalMaxSize;
    
    public void SetupCrystal(
        float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, float _growSpeed, float _crystalMaxSize, Transform _closestTarget){
        crystalExistTimer = _crystalDuration;
        moveSpeed = _moveSpeed;
        canExplode = _canExplode;
        canMove = _canMove;
        crystalGrowSpeed = _growSpeed;
        crystalMaxSize = _crystalMaxSize;
        closestTarget = _closestTarget;
    }
    void Awake() {
        anim = GetComponent<Animator>();
        cd = GetComponent<CircleCollider2D>();
    }
    void Update() {

        crystalExistTimer -= Time.deltaTime;

        if (crystalExistTimer < 0)
            CrystalRelease();
        
        if(canMove){
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, closestTarget.position) < .5f){
                CrystalRelease();
                canMove = false;
            }
        }
        
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(crystalMaxSize, crystalMaxSize), crystalGrowSpeed * Time.deltaTime);
        
    }
    public void CrystalRelease()
    {
        if (canExplode){
            canGrow = true;
            anim.SetTrigger("Explode");
        }
        else
            SelfDestroy();
    }

    private void AnimationExplodeEvent(){

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);

        foreach (var hit in colliders)
        {
            var enemy = hit.GetComponent<Enemy>();

            if(enemy != null)
                enemy.Damage();
        }
    }

    public void SelfDestroy() => Destroy(gameObject);
}