using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyLevelDisplay : MonoBehaviour, IEventListener {

    private TextMeshProUGUI enemyLevelDisplay;

    void Awake() {
        enemyLevelDisplay = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void SetEnemyLevelDisplay(int level) {
        enemyLevelDisplay.SetText("Lv. " + level);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            EnemyStartingDataEvent displayLevelEvent = e as EnemyStartingDataEvent;
            SetEnemyLevelDisplay(displayLevelEvent.level);
        }
    }
}
