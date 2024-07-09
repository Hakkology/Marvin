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

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update() {
        if (canRotate)
            transform.right = rb.velocity;
        
        if (isReturning){
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchTheSword();
        }
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player, float _returnSpeed){
        player = _player;
        returnSpeed = _returnSpeed;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        anim.SetBool("Rotation", true);
    }

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
            
        anim.SetBool("Rotation", false);
        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = other.transform;
    }
}
