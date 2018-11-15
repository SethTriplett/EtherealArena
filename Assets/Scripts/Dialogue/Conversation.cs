using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Conversation : System.Object
{
    [SerializeField] public List<Message> messages;

    public Conversation(List<Message> messages) {
        this.messages = messages;
    }

    public Conversation(Message[] messages) {
        List<Message> messageList = new List<Message>();
        for (int i = 0; i < messages.Length; i++) {
            messageList.Add(messages[i]);
        }
        this.messages = messageList;
    }

}
