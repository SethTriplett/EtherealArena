using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkFlame : MonoBehaviour, IPoolable, IEventListener {

    public enum DarkFlameState {
        idle, fallingFlame
    }

    private DarkFlameState state;
    private float fallingTimer;
    private float period = 1f;
    private float amplitude = 0f;
    private float fallSpeed = 3f;

    private GameObject owner;
    private SpriteRenderer spriteRenderer;
    private bool deactivating;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(DeleteAttacksEvent), this);
        fallingTimer = 0f;
        deactivating = false;
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(DeleteAttacksEvent), this);
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void Update() {
        if (!deactivating) {
            AudioManager.GetInstance().PlaySound(Sound.DarkFlameLoop);
        }
        if (state == DarkFlameState.fallingFlame) {
            Vector3 pos = transform.position;
            float currentFloat = Mathf.Sin(fallingTimer * 2 * Mathf.PI / period) * amplitude;
            float previousFloat = Mathf.Sin((fallingTimer - Time.deltaTime) * 2 * Mathf.PI / period) * amplitude;
            Vector3 deltaPos = new Vector3(currentFloat - previousFloat, -fallSpeed * Time.deltaTime, 0);
            transform.position += deltaPos;

            fallingTimer += Time.deltaTime;
        }
    }

    public void Deactivate() {
        StartCoroutine(DeactivateCoroutine());
    }

    private IEnumerator DeactivateCoroutine() {
        this.deactivating = true;
        AudioManager.GetInstance().StopSound(Sound.DarkFlameLoop);
        while (spriteRenderer.color.a > 0.02) {
            Color targetColor = new Color(1, 1, 1, spriteRenderer.color.a);
            targetColor.a = Mathf.Lerp(targetColor.a, 0, 0.1f);
            spriteRenderer.color = targetColor;
            yield return new WaitForSeconds(0.016f);
        }
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!deactivating) {
            if (other.CompareTag("Player") && other.gameObject.layer != 8) {
                PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
                if (playerStatus != null) {
                    playerStatus.TakeHit();
                } else {
                    Debug.LogError("Player status not found.");
                }
                Deactivate();
            }
        }
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    public void SetFalling() {
        state = DarkFlameState.fallingFlame;
    }

    public void SetIdle() {
        state = DarkFlameState.idle;
    }

    public void SetFalling(float period, float amplitude) {
        state = DarkFlameState.fallingFlame;
        this.period = period;
        this.amplitude = amplitude;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(DeleteAttacksEvent)) {
            DeleteAttacksEvent deleteAttacksEvent = e as DeleteAttacksEvent;
            if (deleteAttacksEvent.owner == this.owner) {
                Deactivate();
            }
        }
    }

}
