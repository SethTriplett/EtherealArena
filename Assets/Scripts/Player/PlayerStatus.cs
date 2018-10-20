using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    private int maxHealth = 3;
    private int currentHealth;
    private float currentEnergy;
    private float invulnerabilityTimer;
    private const float baseInvulnerabilityTime = 4f;
    // Whether or not player sprite should flicker for invulnerability, negative for invisible frames, positive for visibile
    private int flickerFrames;
    private const int baseFlickerRate = 3;
    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer playerArmSpriteRenderer;
    private SpriteRenderer playerHeadSpriteRenderer;

    void Start() {
        currentHealth = maxHealth;
        currentEnergy = 0f;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        playerArmSpriteRenderer = transform.Find("Arm").GetComponent<SpriteRenderer>();
        playerHeadSpriteRenderer = transform.Find("Head").GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (invulnerabilityTimer > 0) {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer > 0) {
                if (flickerFrames < 0) {
                    flickerFrames++;
                    if (flickerFrames == 0) {
                        playerSpriteRenderer.enabled = true;
                        playerArmSpriteRenderer.enabled = true;
                        playerHeadSpriteRenderer.enabled = true;
                        flickerFrames = baseFlickerRate;
                    }
                } else if (flickerFrames > 0) {
                    flickerFrames--;
                    if (flickerFrames == 0) {
                        playerSpriteRenderer.enabled = false;
                        playerArmSpriteRenderer.enabled = false;
                        playerHeadSpriteRenderer.enabled = false;
                        flickerFrames = -baseFlickerRate;
                    }
                }
            } else {
                playerSpriteRenderer.enabled = true;
                playerArmSpriteRenderer.enabled = true;
                playerHeadSpriteRenderer.enabled = true;
                gameObject.layer = 0;
            }
        }
    }

    public void TakeHit() {
        if (invulnerabilityTimer <= 0) {
            currentHealth--;
            EventMessanger.GetInstance().TriggerEvent(new PlayerCurrentHealthEvent(currentHealth));
            gameObject.layer = 8;
            if (currentHealth <= 0) {
                KO();
            } else {
                invulnerabilityTimer = baseInvulnerabilityTime;
                flickerFrames = - baseFlickerRate;
            }
        }
    }

    void RestoreHealth() {
        currentHealth = maxHealth;
        EventMessanger.GetInstance().TriggerEvent(new PlayerCurrentHealthEvent(currentHealth));
    }

    void KO() {
        playerSpriteRenderer.enabled = false;
        playerArmSpriteRenderer.enabled = false;
        EventMessanger.GetInstance().TriggerEvent(new PlayerDefeatEvent());
    }

    public void gainEnergy(float energy) {
        if (energy > 0) {
            currentEnergy += energy;
        }
        EventMessanger.GetInstance().TriggerEvent(new PlayerCurrentEnergyEvent(currentEnergy));
    }


    public int GetHealth() {
        return currentHealth;
    }

}
