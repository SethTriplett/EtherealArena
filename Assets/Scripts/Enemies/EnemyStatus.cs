using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemies health and stats
public class EnemyStatus : MonoBehaviour {

    public int maxHealth = 1;
    private float currentHealth;

    public int defence;
    private bool defeated = false;

    void Start() {
        currentHealth = maxHealth;
        EventMessanger.GetInstance().TriggerEvent(new EnemyMaxHealthEvent(maxHealth));
        EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(maxHealth));
    }

    void Update() {
        if (currentHealth <= 0f) {
            KO();
        }
    }

    void KO() {
        if (!defeated) {
            defeated = true;
            EventMessanger.GetInstance().TriggerEvent(new PlayerVictoryEvent());
        }
    }

    public void TakeDamage(float damage) {
        if (damage > 0) {
            if (defence > 0) {
                damage *= 1 / (1 + (defence / 100f));
                currentHealth -= damage;
            } else {
                damage *= (1 + (- defence / 100));
                currentHealth -= damage;
            }
            EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(currentHealth));
        }
    }

    public void HealHealth(float healthHealed) {
        if (healthHealed > 0) {
            currentHealth += healthHealed;
            if (currentHealth > maxHealth) {
                currentHealth = maxHealth;
            }
            EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(currentHealth));
        }
    }

    public void HealHealthPortion(float portion) {
        if (portion > 0) {
            currentHealth += maxHealth * portion;
            if (currentHealth > maxHealth) {
                currentHealth = maxHealth;
            }
            EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(currentHealth));
        }
    }

}
