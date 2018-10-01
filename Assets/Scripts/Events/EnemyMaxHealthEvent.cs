using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaxHealthEvent : IEvent {

    public int maxHealth;

    public EnemyMaxHealthEvent(int maxHealth) {
        this.maxHealth = maxHealth;
    }

}
