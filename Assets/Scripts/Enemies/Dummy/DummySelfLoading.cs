using System;
using System.Collections;
using UnityEngine;

// This class allows the enemy prefab to wire some of it's own dependencies and behaviors based on parameters.
public class DummySelfLoading : MonoBehaviour, IEventListener {

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void SetStats(int level) {
        EnemyStatus enemyStatus = GetComponent<EnemyStatus>();
        KnifeDummy knifeDummy = GetComponent<KnifeDummy>();
        enemyStatus.maxHealth = 5 * (level + 1);
        knifeDummy.SetSecondForm(level >= 40);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        knifeDummy.SetPlayerTransform(player.transform);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            EnemyStartingDataEvent startingDataEvent = e as EnemyStartingDataEvent;
            if (startingDataEvent.level < 0) startingDataEvent.level = 0;
            SetStats(startingDataEvent.level);
        }
    }

}