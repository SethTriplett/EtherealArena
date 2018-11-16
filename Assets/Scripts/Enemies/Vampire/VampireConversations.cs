using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireConversations : MonoBehaviour, IEventListener {

    private readonly Message[] playerLoss1 = {
        new Message("Vampire", "No match for a count of House Versum.")
    };
    private readonly Message[] playerLoss2 = {
        new Message("Vampire", "No match for a count of House Versum.")
    };
    private readonly Message[] playerLoss3 = {
        new Message("Vampire", "No match for a count of House Versum.")
    };
    private readonly Message[] playerWin1 = {
        new Message("Player", "You'd probably be less pale if you stoped throwing your blood everywere."),
        new Message("Vampire", "Well, yeah but...")
    };
    private readonly Message[] playerWin2 = {
        new Message("Player", "You'd probably be less pale if you stoped throwing your blood everywere."),
        new Message("Vampire", "Well, yeah but...")
    };
    private readonly Message[] playerWin3 = {
        new Message("Player", "You'd probably be less pale if you stoped throwing your blood everywere."),
        new Message("Vampire", "Well, yeah but...")
    };

    private Conversation[,] conversations = new Conversation[4, 2];

    void Awake() {
        conversations[1, 0] = new Conversation(playerLoss1);
        conversations[2, 0] = new Conversation(playerLoss2);
        conversations[3, 0] = new Conversation(playerLoss3);
        conversations[1, 1] = new Conversation(playerWin1);
        conversations[2, 1] = new Conversation(playerWin2);
        conversations[3, 1] = new Conversation(playerWin3);
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PostBattleDialogStartEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PostBattleDialogStartEvent), this);
    }

    private void Converse(int phase, bool playerVictory) {
        int victoryIndex = playerVictory ? 1 : 0;
        List<Conversation> conversationList = new List<Conversation>();
        if (phase < conversations.GetLength(0) && victoryIndex < conversations.GetLength(1) &&
            conversations[phase, victoryIndex] != null) {
            conversationList.Add(conversations[phase, victoryIndex]);
        } else {
            Message[] defaultMessage = { new Message("Player", "It's over!") };
            conversationList.Add(new Conversation(defaultMessage));
        }
        DialogueManager.Instance.SetConversationList(conversationList);
        DialogueManager.Instance.NextConversation();
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PostBattleDialogStartEvent)) {
            PostBattleDialogStartEvent dialogStartEvent = e as PostBattleDialogStartEvent;
            Converse(dialogStartEvent.phase, dialogStartEvent.playerVictory);
        }
    }

}
