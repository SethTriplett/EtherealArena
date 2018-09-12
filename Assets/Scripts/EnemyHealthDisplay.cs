using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthDisplay : MonoBehaviour {

    private Slider healthSlider;
    private Image sliderFill;
    private TextMeshProUGUI currentHP;
    private TextMeshProUGUI maxHP;

    private int maxHealth;

    void Start() {
        healthSlider = transform.Find("Enemy Health").GetComponent<Slider>();
        sliderFill = healthSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        currentHP = transform.Find("Current HP").GetComponent<TextMeshProUGUI>();
        maxHP = transform.Find("Max HP").GetComponent<TextMeshProUGUI>();
    }

    public void SetHealth(float health) {
        healthSlider.value = health;
        float proportion = health / maxHealth;
        Color fillColor = Color.HSVToRGB(proportion * 135 / 360f, 1f, 1f);
        sliderFill.color = fillColor;
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
}
