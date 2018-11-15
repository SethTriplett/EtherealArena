using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message : System.Object
{
    [SerializeField] public string speaker;
    [SerializeField] public string text;

    public Message (string speaker, string text) {
        this.speaker = speaker;
        this.text = text;
    }

}
