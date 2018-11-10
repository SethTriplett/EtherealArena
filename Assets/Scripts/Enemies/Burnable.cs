using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour {

    private float temperature;
    private const float BASE_TEMPERATURE = 37f;
    private EnemyStatus enemyStatus;
    private float burnTimer; //Time after burn until temperature decreases
    private const float BASE_BURN_TIMER = 1f;

    void Awake() {
        enemyStatus = GetComponent<EnemyStatus>();
        if (enemyStatus == null) {
            Debug.LogError("Enemy Status not found for burnable.");
        }
    }

    void FixedUpdate() {
        burnTimer -= Time.deltaTime;
        if (temperature > 37f) {
            if (burnTimer <= -1) {
                temperature -= 0.4f;
            } else if (burnTimer <= 0) {
                temperature -= 0.2f;
            }
        }
    }

    void OnEnable() {
        temperature = BASE_TEMPERATURE;
    }

    public void Burn(float baseDamage) {
        temperature += 0.2f;
        float temperatureScale = (temperature - BASE_TEMPERATURE + 20f) / 40f;
        enemyStatus.TakeDamage(baseDamage * temperatureScale);
        burnTimer = BASE_BURN_TIMER;
    }

}
