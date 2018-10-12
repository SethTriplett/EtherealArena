using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCurrentHealthEvent : IEvent {

    public float currentHealth;

    public EnemyCurrentHealthEvent(float health) {
        this.currentHealth = health;
    }

}
