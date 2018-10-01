using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class VictoryScreen : MonoBehaviour, IEventListener {

    private CanvasGroup victoryScreen;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerVictoryEvent), this);
    }
    
    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerVictoryEvent), this);
    }

    void Awake() {
        victoryScreen = GetComponent<CanvasGroup>();
    }

    void DisplayVictoryScreen() {
        victoryScreen.alpha = 1;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PlayerVictoryEvent)) {
            DisplayVictoryScreen();
        }
    }

}
