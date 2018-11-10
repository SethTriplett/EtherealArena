using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyNameDisplay : MonoBehaviour, IEventListener {

    private TextMeshProUGUI enemyNameDisplay;

    void Awake() {
        enemyNameDisplay = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void SetEnemyNameDisplay(string name) {
        enemyNameDisplay.SetText(name);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            EnemyStartingDataEvent displayLevelEvent = e as EnemyStartingDataEvent;
            SetEnemyNameDisplay(displayLevelEvent.name);
        }
    }
}
