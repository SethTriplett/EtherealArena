using UnityEngine;

public class DeleteAttacksEvent : IEvent {
    
    public GameObject owner;

    public DeleteAttacksEvent(GameObject owner) {
        this.owner = owner;
    }

}