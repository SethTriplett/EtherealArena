using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour, IEventListener {

    private bool victory = false;
    private bool defeat = false;


    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerDefeatEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerDefeatEvent), this);
    }

    void PlayerVictory() {
        victory = true;
    }

    void OpponentVictory() {
        defeat = true;
    }

    void Update() {
        if (victory || defeat) {
            if (Input.GetButtonUp("A")) {
                SceneManager.LoadScene("WorldMap");
            }
        }
    }

    public void Reset() {
        victory = false;
        defeat = false;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PlayerVictoryEvent)) {
            PlayerVictory();
        } else if (e.GetType() == typeof(PlayerDefeatEvent)) {
            OpponentVictory();
        }
    }

}
