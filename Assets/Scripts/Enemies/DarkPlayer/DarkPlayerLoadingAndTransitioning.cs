using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPlayerLoadingAndTransitioning : MonoBehaviour, IEventListener {

    private int level;
    private int maxPhase;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PhaseTransitionEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PhaseTransitionEvent), this);
    }
    
    public int GetPhaseMaxHP(int nextPhase, int level, int maxPhase) {
        float totalHealth = 100 + 3 * level;
        if (nextPhase == 1) {
            if (maxPhase == 1) {
                return (int) totalHealth;
            } else if (maxPhase == 2) {
                return Mathf.FloorToInt(totalHealth * 3.5f / 10f);
            } else if (maxPhase == 3) {
                return Mathf.FloorToInt(totalHealth * 3.75f / 10f);
            }
        } else if (nextPhase == 2) {
            if (maxPhase == 2) {
                return Mathf.FloorToInt(totalHealth * 6.5f / 10f);
            } else if (maxPhase == 3) {
                return Mathf.FloorToInt(totalHealth * 4.25f / 10f);
            }
        } else if (nextPhase == 3) {
            return Mathf.FloorToInt(totalHealth * 3 / 10f);
        }
        Debug.LogError("Wrong phase given.");
        return 0;
    }

    void SetStats(int phase, int level, int maxPhase) {
        EnemyStatus enemyStatus = GetComponent<EnemyStatus>();
        if (maxPhase > 0) {
            enemyStatus.SetMaxPhase(maxPhase);
        } else {
            if (level < 30) {
                enemyStatus.SetMaxPhase(1);
                maxPhase = 1;
            } else if (level < 60) {
                enemyStatus.SetMaxPhase(2);
                maxPhase = 2;
            } else {
                enemyStatus.SetMaxPhase(3);
                maxPhase = 3;
            }
        }
        enemyStatus.maxHealth = GetPhaseMaxHP(phase, level, maxPhase);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            EnemyStartingDataEvent startingDataEvent = e as EnemyStartingDataEvent;
            if (startingDataEvent.level < 0) startingDataEvent.level = 0;
            SetStats(1, startingDataEvent.level, startingDataEvent.maxPhase);
            this.level = startingDataEvent.level;
            this.maxPhase = startingDataEvent.maxPhase;
        } else if (e.GetType() == typeof(PhaseTransitionEvent)) {
            PhaseTransitionEvent phaseTransitionEvent = e as PhaseTransitionEvent;
            SetStats(phaseTransitionEvent.nextPhase, level, maxPhase);
        }
    }

}
