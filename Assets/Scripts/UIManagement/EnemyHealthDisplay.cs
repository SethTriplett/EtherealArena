using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthDisplay : MonoBehaviour, IEventListener {

    private Slider healthSlider;
    private Slider phase2Bar;
    private Slider phase3Bar;
    private Image sliderFill;
    private TextMeshProUGUI currentHP;
    private TextMeshProUGUI maxHP;

    private int maxHealth;
    // Used to allow health bars to transition
    private float lerpHealth;
    private float currentHealth;

    private float maxPhase;
    private float currentPhase;

    private bool transitioning;

    void Awake() {
        healthSlider = transform.Find("Enemy Health").GetComponent<Slider>();
        phase2Bar = transform.Find("Phase 2 Bar").GetComponent<Slider>();
        phase3Bar = transform.Find("Phase 3 Bar").GetComponent<Slider>();
        sliderFill = healthSlider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>();
        currentHP = transform.Find("Current HP").GetComponent<TextMeshProUGUI>();
        maxHP = transform.Find("Max HP").GetComponent<TextMeshProUGUI>();
        transitioning = false;

        AddExtraBars();
    }

    void OnEnable() {
        EventMessanger.instance.SubscribeEvent(typeof(EnemyMaxHealthEvent), this);
        EventMessanger.instance.SubscribeEvent(typeof(EnemyCurrentHealthEvent), this);
        EventMessanger.instance.SubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.instance.SubscribeEvent(typeof(EnemyHealthTransitionEvent), this);
    }

    void OnDisable() {
        EventMessanger.instance.UnsubscribeEvent(typeof(EnemyMaxHealthEvent), this);
        EventMessanger.instance.UnsubscribeEvent(typeof(EnemyCurrentHealthEvent), this);
        EventMessanger.instance.UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.instance.UnsubscribeEvent(typeof(EnemyHealthTransitionEvent), this);
    }

    void Update() {
        if (!transitioning) {
            LerpHealth();
        }
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

    // Add extra bars for extra phases
    private void AddExtraBars() {
        if (maxPhase <= 1) {
            phase2Bar.gameObject.SetActive(false);
            phase3Bar.gameObject.SetActive(false);
        }
        if (maxPhase >= 2) {
            phase2Bar.gameObject.SetActive(true);
        }
        if (maxPhase >= 3) {
            phase3Bar.gameObject.SetActive(true);
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

    private IEnumerator PhaseTransitionHealth(float duration, int nextPhase) {
        transitioning = true;
        if (nextPhase != 2 && nextPhase != 3) {
            Debug.LogError("Weird phase given to transition");
        } else if (nextPhase == 2) {
            Color fillColor = Color.HSVToRGB(135 / 360f, 1f, 1f);
            sliderFill.color = fillColor;
            for (float x = 0; x < duration; x += Time.deltaTime) {
                float value = Mathf.SmoothStep(0, 1, x/duration);
                phase2Bar.value = (1 - value);
                healthSlider.value = value * healthSlider.maxValue;
                SetDisplayHealth(Mathf.CeilToInt(value * healthSlider.maxValue));
                yield return null;
            }
            lerpHealth = maxHealth;
            SetHealth(maxHealth);
        } else if (nextPhase == 3) {
            Color fillColor = Color.HSVToRGB(135 / 360f, 1f, 1f);
            sliderFill.color = fillColor;
            for (float x = 0; x < duration; x += Time.deltaTime) {
                float value = Mathf.SmoothStep(0, 1, x/duration);
                phase3Bar.value = (1 - value);
                healthSlider.value = value * healthSlider.maxValue;
                SetDisplayHealth(Mathf.CeilToInt(value * healthSlider.maxValue));
                yield return null;
            }
            // So that we don't lerp after filling
            lerpHealth = maxHealth;
            SetHealth(maxHealth);
        }
        transitioning = false;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyCurrentHealthEvent)) {
            if (!transitioning) {
                EnemyCurrentHealthEvent currentHealthEvent = e as EnemyCurrentHealthEvent;
                SetHealth(currentHealthEvent.currentHealth);
                SetDisplayHealth(Mathf.CeilToInt(currentHealthEvent.currentHealth));
            }
        } else if (e.GetType() == typeof(EnemyMaxHealthEvent)) {
            EnemyMaxHealthEvent maxHealthEvent = e as EnemyMaxHealthEvent;
            SetMaxHealth(maxHealthEvent.maxHealth);
        } else if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            EnemyStartingDataEvent startingDataEvent = e as EnemyStartingDataEvent;
            this.maxPhase = startingDataEvent.maxPhase;
            AddExtraBars();
        } else if (e.GetType() == typeof(EnemyHealthTransitionEvent)) {
            EnemyHealthTransitionEvent transitionEvent = e as EnemyHealthTransitionEvent;
            StartCoroutine(PhaseTransitionHealth(transitionEvent.duration, transitionEvent.nextPhase));
        }
    }

}
