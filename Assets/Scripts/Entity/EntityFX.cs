using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sRenderer;

    [Header("Flash Effects")]
    [SerializeField] private Material hitMat;
    private Material originMat;

    void Start() {
        sRenderer = GetComponentInChildren<SpriteRenderer>();
        originMat = sRenderer.material;
    }

    public IEnumerator FlashFX(){
        sRenderer.material = hitMat;
        yield return new WaitForSeconds(.5f);
        sRenderer.material = originMat;
    }
}
