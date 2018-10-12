using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour {

    // Handles enemies health and stats

    public int maxHealth = 1;
    private float currentHealth;

    public int attack;
    public int defence;
    [SerializeField] private EnemyHealthDisplay UIDisplay;
    [SerializeField] private SceneManagement sceneManagement;
    private KnifeDummy knifeDummy;
    private bool defeated = false;

    void Start() {
        currentHealth = maxHealth;
        UIDisplay.SetMaxHealth(maxHealth);
        UIDisplay.SetHealth(currentHealth);
        UIDisplay.SetDisplayHealth(maxHealth);

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
            sceneManagement.PlayerVictory();

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
            UIDisplay.SetHealth(currentHealth);
            UIDisplay.SetDisplayHealth((int) Mathf.Ceil(currentHealth));
        }
    }

    // Deal damage ignoring defence
    public void TakeRawDamage(float damage) {
        if (damage > 0) {
            currentHealth -= damage;
            UIDisplay.SetHealth(currentHealth);
        }
    }

}
