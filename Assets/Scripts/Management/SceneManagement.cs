using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour, IEventListener {

    private bool victory = false;
    private bool defeat = false;

    // Tells it to load the world map after the post battle conversation
    private bool loadAfterConversation = false;


    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerDefeatEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(ConversationEndEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerDefeatEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(ConversationEndEvent), this);
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
                victory = false;
                defeat = false;
                Vignette.LoadScene("WorldMap");
            }
        }
    }

    public void Reset() {
        victory = false;
        defeat = false;
    }

    public void ReturnToWorldMap() {
        Vignette.LoadScene("WorldMap");
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PlayerVictoryEvent)) {
            //PlayerVictory();
            loadAfterConversation = true;
        } else if (e.GetType() == typeof(PlayerDefeatEvent)) {
            //OpponentVictory();
            loadAfterConversation = true;
        } else if (e.GetType() == typeof(ConversationEndEvent)) {
            if (loadAfterConversation) ReturnToWorldMap();
        }
    }

}
