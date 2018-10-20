using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthDisplay : MonoBehaviour, IEventListener {

    private Slider healthSlider;
    private Image sliderFill;
    private TextMeshProUGUI currentHP;
    private TextMeshProUGUI maxHP;

    private int maxHealth;
    // Used to allow health bars to transition
    private float lerpHealth;
    private float currentHealth;

    private float LerpSpeed;

    void Awake() {
        healthSlider = transform.Find("Enemy Health").GetComponent<Slider>();
        sliderFill = healthSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        currentHP = transform.Find("Current HP").GetComponent<TextMeshProUGUI>();
        maxHP = transform.Find("Max HP").GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() {
        EventMessanger.instance.SubscribeEvent(typeof(EnemyMaxHealthEvent), this);
        EventMessanger.instance.SubscribeEvent(typeof(EnemyCurrentHealthEvent), this);
    }

    void OnDisable() {
        EventMessanger.instance.UnsubscribeEvent(typeof(EnemyMaxHealthEvent), this);
        EventMessanger.instance.UnsubscribeEvent(typeof(EnemyCurrentHealthEvent), this);
    }

    void Update() {
        LerpHealth();
    }

    private void LerpHealth() {
        // Lerp display health to real health
        if (lerpHealth != currentHealth) {
            lerpHealth = Mathf.Lerp(lerpHealth, currentHealth, 0.1f);
            if (Mathf.Abs(lerpHealth - currentHealth) < 0.0001) lerpHealth = currentHealth;
            healthSlider.value = lerpHealth;
            float proportion = lerpHealth / maxHealth;
            Color fillColor = Color.HSVToRGB(proportion * 135 / 360f, 1f, 1f);
            sliderFill.color = fillColor;
        }
    }

    public void SetHealth(float health) {
        currentHealth = health;
        LerpHealth();
    }

    public void SetDisplayHealth(int displayHealth) {
        if (displayHealth < 0) {
            displayHealth = 0;
        }
        currentHP.SetText(displayHealth.ToString());
    }

    public void SetMaxHealth(int maxHealth) {
        this.maxHealth = maxHealth;
        maxHP.SetText(maxHealth.ToString());
        healthSlider.maxValue = maxHealth;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyCurrentHealthEvent)) {
            EnemyCurrentHealthEvent currentHealthEvent = e as EnemyCurrentHealthEvent;
            SetHealth(currentHealthEvent.currentHealth);
            SetDisplayHealth(Mathf.CeilToInt(currentHealthEvent.currentHealth));
        } else if (e.GetType() == typeof(EnemyMaxHealthEvent)) {
            EnemyMaxHealthEvent maxHealthEvent = e as EnemyMaxHealthEvent;
            SetMaxHealth(maxHealthEvent.maxHealth);
        }
    }

}
