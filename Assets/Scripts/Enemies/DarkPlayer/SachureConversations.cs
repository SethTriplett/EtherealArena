using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SachureConversations : MonoBehaviour, IEventListener {

    private readonly Message[] playerLoss1 = {
        new Message("Sachure", "I can see your heart. So cold.")
    };
    private readonly Message[] playerLoss2 = {
        new Message("Sachure", "No fire could melt this heart of stone.")
    };
    private readonly Message[] playerLoss3 = {
        new Message("Sachure", "Your mind... an eternal battlefield.")
    };
    private readonly Message[] playerWin3 = {
        new Message("Sachure", "Your heart is... warm... and strong.")
    };

    private Conversation[,] conversations = new Conversation[4, 2];

    void Awake() {
        conversations[1, 0] = new Conversation(playerLoss1);
        conversations[2, 0] = new Conversation(playerLoss2);
        conversations[3, 0] = new Conversation(playerLoss3);
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
            if (dialogStartEvent.phase == 3 && dialogStartEvent.playerVictory == true) {
                EventMessanger.GetInstance().TriggerEvent(new FinalBossDefeatedEvent());
            }
            Converse(dialogStartEvent.phase, dialogStartEvent.playerVictory);
        }
    }

}
