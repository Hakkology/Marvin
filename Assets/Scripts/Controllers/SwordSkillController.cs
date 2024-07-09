using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private float returnSpeed;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;


    [Header("Bounce Info")]
    private bool isBouncing;
    private int amountOfBounce;

    private List<Transform> enemyTarget;
    private int targetIndex;
    private int remainingBounce;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }

        ApplyBounce();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _returnSpeed){
        player = _player;
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        anim.SetBool("Rotation", true);
    }

    #region Bounce
    public void SetupBounce(bool _isBouncing, int _amountOfBounce){
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;
        remainingBounce = amountOfBounce;
        enemyTarget = new List<Transform>();
    }

    private void ApplyBounce()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, returnSpeed * Time.deltaTime);
            // bounce speed set to return speed, dont forget.

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                targetIndex++;
                remainingBounce--;

                if (remainingBounce == 0)
                {
                    isBouncing = false;
                    isReturning = true;
                    remainingBounce = amountOfBounce;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }
    #endregion

    public void ReturnSword(){
        //rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
        anim.SetBool("Rotation", true);
    }

    void OnTriggerEnter2D(Collider2D other) {

        if(isReturning)
            return;
        
        if(other.GetComponent<Enemy>() != null){

            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }

        StuckInto(other);
    }

    private void StuckInto(Collider2D other)
    {
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }
}
