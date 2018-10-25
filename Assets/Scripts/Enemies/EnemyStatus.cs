using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemies health and stats
public class EnemyStatus : MonoBehaviour {

    public int maxHealth = 1;
    private float currentHealth;

    // The current phase of the boss. 1 in the first phase.
    [SerializeField] private int currentPhase;
    // The number of phases in a boss. 3 at most.
    [SerializeField] private int maxPhase;

    private bool defeated = false;
    private bool invulnerable = false;

    private IPhaseTransition phaseTransitioner;
    private int level = 5;

    void Start() {
        currentHealth = maxHealth;
        phaseTransitioner = GetComponent<IPhaseTransition>();
        EventMessanger.GetInstance().TriggerEvent(new EnemyMaxHealthEvent(maxHealth));
        EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(maxHealth));
    }

    void Update() {
        if (currentHealth <= 0f) {
            if (currentPhase >= maxPhase) {
                KO();
            } else {
                currentPhase++;
                TransitionPhases(currentPhase);
            }
        }
    }

    void KO() {
        if (!defeated) {
            defeated = true;
            EventMessanger.GetInstance().TriggerEvent(new PlayerVictoryEvent());
        }
    }

    void TransitionPhases(int nextPhase) {
        maxHealth = phaseTransitioner.GetPhaseMaxHP(nextPhase, this.level);
        currentHealth = maxHealth;
        EventMessanger.GetInstance().TriggerEvent(new EnemyMaxHealthEvent(maxHealth));
        EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(currentHealth));
        phaseTransitioner.PhaseTransition(nextPhase);
    }

    public void TakeDamage(float damage) {
        /* Defence is no longer planned to be implemented
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
        */
        if (damage > 0 && !invulnerable) {
            currentHealth -= damage;
        }
        EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(currentHealth));
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

    public int getPhase()
    {
        return currentPhase;
    }

}
