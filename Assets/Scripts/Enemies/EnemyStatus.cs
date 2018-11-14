using GameJolt.API.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles enemies health and stats
public class EnemyStatus : MonoBehaviour, IEventListener {

    public int maxHealth;
    private float currentHealth;

    // The current phase of the boss. 1 in the first phase.
    [SerializeField] private int currentPhase;
    // The number of phases in a boss. 3 at most.
    [SerializeField] private int maxPhase;

    private bool defeated = false;
    private bool invulnerable = false;
    private float invulnerabilityTimer = 0f;

    void Start() {
        currentHealth = maxHealth;
        EventMessanger.GetInstance().TriggerEvent(new EnemyMaxHealthEvent(maxHealth));
        EventMessanger.GetInstance().TriggerEvent(new EnemyCurrentHealthEvent(maxHealth));
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void Update() {
        if (invulnerabilityTimer > 0f) {
            invulnerable = true;
            invulnerabilityTimer -= Time.deltaTime;
        } else {
            invulnerable = false;
        }
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
            
            LoadBattleSceneScript loadBattleSceneScript = FindObjectOfType<LoadBattleSceneScript>();
            GameJolt.API.Scores.Add(
                new Score((int)Time.timeSinceLevelLoad, 
                Time.timeSinceLevelLoad.ToString(), "Someone w/ " + SystemInfo.deviceModel), 
                HighscoresManager.GetHighscoresIndex(loadBattleSceneScript.EnemyType, loadBattleSceneScript.EnemyLevel), isSuccess => Debug.Log("Highscores update success=" + isSuccess));
        }
    }

    void TransitionPhases(int nextPhase) {
        EventMessanger.GetInstance().TriggerEvent(new PhaseTransitionEvent(nextPhase));
        invulnerabilityTimer = 1.5f;
        currentHealth = maxHealth;
        EventMessanger.GetInstance().TriggerEvent(new EnemyMaxHealthEvent(maxHealth));
        EventMessanger.GetInstance().TriggerEvent(new EnemyHealthTransitionEvent(1.5f, nextPhase));
        EventMessanger.GetInstance().TriggerEvent(new PhaseTransitionEvent(nextPhase));
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

    public void SetMaxPhase(int phase) {
        this.maxPhase = phase;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            EnemyStartingDataEvent dataEvent = e as EnemyStartingDataEvent;
            this.maxPhase = dataEvent.maxPhase;
        }
    }

}
