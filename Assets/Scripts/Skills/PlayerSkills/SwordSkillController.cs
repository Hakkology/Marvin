using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
        anim = GetComponentInChildren<Animator>();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale){
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
    }
}
