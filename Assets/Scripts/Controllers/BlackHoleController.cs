using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour {
    public float maxSize = 15;
    public float growSpeed = 1;
    public bool canGrow;

    public List<Transform> targets;

    void Update() {
        if (canGrow)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        
    }

    void OnTriggerEnter2D(Collider2D other) {

        Enemy enemy = other.GetComponent<Enemy>();

        if(enemy != null)
            enemy.FreezeTime(true);
        
        
        
    }
}