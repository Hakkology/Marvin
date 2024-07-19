using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour {
    private Entity entity;
    private EntityStats entityStats;
    private RectTransform healthBarTransform;
    private Slider healthSlider;

    void Start() {
        healthBarTransform = GetComponent<RectTransform>();
        healthSlider = GetComponentInChildren<Slider>();
        entity = GetComponentInParent<Entity>();
        entityStats = GetComponentInParent<EntityStats>();

        entity.onFlipped += FlipUI;
        entityStats.onHealthChanged += UpdateHealthUI;

        UpdateHealthUI();
    }

    void Update() => UpdateHealthUI();
    private void FlipUI() => healthBarTransform.Rotate(0, 180, 0);
    void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        entityStats.onHealthChanged -= UpdateHealthUI;
    }

    void UpdateHealthUI(){
        healthSlider.maxValue = entityStats.GetMaxHealthValue();
        healthSlider.value = entityStats.currentHealth;
    }
    
}