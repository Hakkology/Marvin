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

    [Header("Pierce Info")]
    private float pierceAmount;


    [Header("Bounce Info")]
    private bool isBouncing;
    private int bounceAmount;
    private List<Transform> enemyTarget;
    private int targetIndex;
    private int remainingBounce;

    [Header("Spin Info")]
    private float maxTravelDistance;
    private float spinDuration;
    private float spinTimer;
    private bool wasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

    [Header("Freeze Info")]
    private float freezeTimeDuration;



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
        ApplySpin();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isReturning)
            return;

        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
            SwordSkillDamage(enemy);

        SetupTargetForBounce(other);
        StuckInto(other);
    }

    #region Spin

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown){
        isSpinning = _isSpinning;
        maxTravelDistance = _maxTravelDistance;
        spinDuration = _spinDuration;
        hitCooldown = _hitCooldown;
    }

    private void StopWhenSpinning()
    {
        wasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        spinTimer = spinDuration;
    }

    private void ApplySpin()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
                StopWhenSpinning();

            if (wasStopped)
            {
                spinTimer -= Time.deltaTime;

                if (spinTimer < 0)
                {

                    isReturning = true;
                    isSpinning = false;
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer < 0)
                {
                    hitTimer = hitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        Enemy enemy = hit.GetComponent<Enemy>();
                        if (enemy != null)
                            SwordSkillDamage(enemy);
                    }
                }
            }
        }
    }

    #endregion

    #region Pierce

    public void SetupPierce(float _pierceAmount){
        pierceAmount = _pierceAmount;
    }

    #endregion

    #region Bounce
    public void SetupBounce(bool _isBouncing, int _amountOfBounce){
        isBouncing = _isBouncing;
        bounceAmount = _amountOfBounce;
        remainingBounce = bounceAmount;
        enemyTarget = new List<Transform>();
    }
    private void SetupTargetForBounce(Collider2D other)
    {
        if (other.GetComponent<Enemy>() != null)
        {

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
    }
    private void ApplyBounce()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, returnSpeed * Time.deltaTime);
            // bounce speed set to return speed, dont forget.

            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                Enemy enemy = enemyTarget[targetIndex].GetComponent<Enemy>();

                if (enemy != null)
                    SwordSkillDamage(enemy);
                
                targetIndex++;
                remainingBounce--;

                if (remainingBounce == 0)
                {
                    isBouncing = false;
                    isReturning = true;
                    remainingBounce = bounceAmount;
                }

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    #endregion

    #region Sword
    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _returnSpeed, float _freezeTimeDuration){
        player = _player;
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        freezeTimeDuration = _freezeTimeDuration;

        if(pierceAmount <=0)
            anim.SetBool("Rotation", true);
        
        Invoke("DestroyMe", 7);
    }
    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }
    private void DestroyMe() => Destroy(gameObject);
    
    public void ReturnSword(){
        //rb.isKinematic = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
        anim.SetBool("Rotation", true);
    }

    private void StuckInto(Collider2D other)
    {
        if(pierceAmount > 0 && other.GetComponent<Enemy>() != null){
            pierceAmount--;
            return;
        }

        if(isSpinning){
            StopWhenSpinning();
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = other.transform;
    }

    #endregion

}
