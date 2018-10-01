using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {

    // Handles enemies health and stats

    public int maxHealth = 1;
    private float currentHealth;

    public int attack;
    public int defence;
    private KnifeDummy knifeDummy;
    private bool defeated = false;

    void Start() {
        currentHealth = maxHealth;
        EventMessanger.GetInstance().TriggerEvent(new EnemyMaxHealthEvent(maxHealth));
        EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(maxHealth));

        // temporary
        knifeDummy = GetComponent<KnifeDummy>();
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

            // temporary
            knifeDummy.StopAttacking();
            knifeDummy.StopAllCoroutines();
            transform.Rotate(0f, 0f, 90f);
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

}
