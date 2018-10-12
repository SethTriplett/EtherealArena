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

    public void HealHealth()
    {
        if(currentHealth < maxHealth)
        {
            currentHealth = currentHealth + (int) (maxHealth / 10 < 1 ? 1 : maxHealth / 10);
            EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(currentHealth));
        }
    }

}
