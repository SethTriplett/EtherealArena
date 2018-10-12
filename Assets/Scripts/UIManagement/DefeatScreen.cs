using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class BattleEndScreens : MonoBehaviour, IEventListener {

    private CanvasGroup defeatScreen;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerDefeatEvent), this);
    }
    
    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerDefeatEvent), this);
    }

    void Awake() {
        defeatScreen = GetComponent<CanvasGroup>();
    }

    void DisplayDefeatScreen() {
        defeatScreen.alpha = 1;
    }

    public void ConsumeEvent(IEvent e) {
        print(e);
        if (e.GetType() == typeof(PlayerDefeatEvent)) {
            DisplayDefeatScreen();
        }
    }

}
