using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    [SerializeField]
    private float flashAnimationTime = .5f;
    private SpriteRenderer sRenderer;

    [Header("Flash Effects")]
    [SerializeField] private Material hitMat;
    private Material originMat;

    [Header("Ailment Colors")]
    [SerializeField] private Color[] chillColors;
    [SerializeField] private Color[] igniteColors;
    [SerializeField] private Color[] shockColors;

    void Start() {
        sRenderer = GetComponentInChildren<SpriteRenderer>();
        originMat = sRenderer.material;
    }

    public IEnumerator FlashFX(){

        sRenderer.material = hitMat;
        Color currentColor = sRenderer.color;

        sRenderer.color = Color.white;
        yield return new WaitForSeconds(flashAnimationTime);

        sRenderer.color = currentColor;
        sRenderer.material = originMat;
    }

    private void RedColorBlink(){
        if(sRenderer.color != Color.white) sRenderer.color = Color.white;
        else sRenderer.color = Color.red;
    }

    private void CancelColorChange(){
        CancelInvoke();
        sRenderer.color = Color.white;
    }

    public void IgniteFXFor(float _seconds){
        InvokeRepeating(nameof(IgniteColorFX), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }

    private void IgniteColorFX(){
        if (sRenderer.color != igniteColors[0]) sRenderer.color = igniteColors[0];
        else sRenderer.color = igniteColors[1];
    }

    public void ChillFxFor(float _seconds){
        InvokeRepeating(nameof(ChillColorFX), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }
    private void ChillColorFX(){
        if (sRenderer.color != chillColors[0]) sRenderer.color = chillColors[0];
        else sRenderer.color = chillColors[1];
    }
    public void ShockFXFor(float _seconds){
        InvokeRepeating(nameof(ShockColorFX), 0, .3f);
        Invoke(nameof(CancelColorChange), _seconds);
    }
    private void ShockColorFX(){
        if (sRenderer.color != shockColors[0]) sRenderer.color = shockColors[0];
        else sRenderer.color = shockColors[1];
    }

    public void TurnInvisible(bool _invisible){
        if(_invisible) sRenderer.color = Color.clear;
        else sRenderer.color = Color.white;
    }


}
