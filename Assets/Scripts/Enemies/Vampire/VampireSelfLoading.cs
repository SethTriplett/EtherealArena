using System;
using System.Collections;
using UnityEngine;

// This class allows the enemy prefab to wire some of it's own dependencies and behaviors based on parameters.
public class VampireSelfLoading : MonoBehaviour, IEventListener {

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void SetStats(int level, int maxPhase) {
        EnemyStatus enemyStatus = GetComponent<EnemyStatus>();
        VampireController vampireController = GetComponent<VampireController>();
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
        float totalHealth = 5 * (level + 1);
        if (maxPhase == 1) {
            enemyStatus.maxHealth = (int) totalHealth;
        } else if (maxPhase == 2) {
            enemyStatus.maxHealth = Mathf.FloorToInt(totalHealth * 4f / 10f);
        } else if (maxPhase == 3) {
            enemyStatus.maxHealth = Mathf.FloorToInt(totalHealth * 2f / 10f);
        }
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        vampireController.SetPlayerTransform(player.transform);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            EnemyStartingDataEvent startingDataEvent = e as EnemyStartingDataEvent;
            if (startingDataEvent.level < 0) startingDataEvent.level = 0;
            SetStats(startingDataEvent.level, startingDataEvent.maxPhase);
        }
    }

}