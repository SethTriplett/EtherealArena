using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour, IEventListener, IPoolable {

    private GameObject owner;
    private bool deactivating;
    private SpriteRenderer spriteRenderer;
    private readonly Vector3 STARTING_SCALE = new Vector3(0.2f, 0.2f, 0.2f);
    private readonly float START_SPEED = 17f;
    private readonly float TARGET_SPEED = 2f;
    private readonly float LERP_RATE = 0.1f;
    private readonly float SCALE_UP_RATE = 4f;
    private readonly float DISAPPEAR_RATE = 0.15f;  //Independent of deactivate
    private readonly float BASE_TIME = 2f;
    private float speed;
    private float angle;
    private float radAngle;
    private float remainingTime;
    private float baseDamage = 1f;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable() {
        this.deactivating = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        speed = START_SPEED;
        transform.localScale = STARTING_SCALE;
        remainingTime = BASE_TIME;
        spriteRenderer.sortingOrder = 200;
        SetAnglesFromRotation();

        EventMessanger.GetInstance().SubscribeEvent(typeof(DeleteAttacksEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(DeleteAttacksEvent), this);
    }

    void Update() {
        transform.position += Vector3.right * Mathf.Cos(radAngle) * speed * Time.deltaTime;
        transform.position += Vector3.up * Mathf.Sin(radAngle) * speed * Time.deltaTime;
        transform.localScale += Vector3.one * SCALE_UP_RATE * Time.deltaTime;
        transform.rotation = Quaternion.Euler(Vector3.zero);

        float alpha = spriteRenderer.color.a;
        alpha -= DISAPPEAR_RATE * Time.deltaTime;
        spriteRenderer.color = new Color(1, 1, 1, alpha);

        StartCoroutine(DecreaseSpeed());

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f && !deactivating) {
            StartCoroutine(Deactivate());
        }

        spriteRenderer.sortingOrder--;
    }

    // In a coroutine in an effort to be frame rate independent
    IEnumerator DecreaseSpeed() {
        while (Mathf.Abs(speed - TARGET_SPEED) > 0.01f) {
            speed = Mathf.Lerp(speed, TARGET_SPEED, LERP_RATE);
            yield return new WaitForSeconds(0.016f);
        }
    }

    void SetAnglesFromRotation() {
        angle = transform.rotation.eulerAngles.z;
        radAngle = angle * Mathf.PI / 180f;
    }

    void OnTriggerEnter2D(Collider2D other) {
        // if colliding with an enemy, deal damage play hit animation
        if (other.CompareTag("Enemy") && !deactivating) {
            Burnable burnableScript = other.GetComponent<Burnable>();
            if (burnableScript != null) {
                burnableScript.Burn(baseDamage);
            } else {
                Debug.LogError("Enemy not burning.");
            }
            if (owner != null) {
                PlayerStatus playerStatusReference = owner.GetComponent<PlayerStatus>();
                if (playerStatusReference != null) playerStatusReference.gainEnergy(1f);
            }
            speed = 0f;
            StartCoroutine(Deactivate());
        }
        else if (other.transform.parent.gameObject.CompareTag("bat"))
        {
            other.transform.parent.gameObject.SetActive(false);
        }
    }

    IEnumerator Deactivate() {
        this.deactivating = true;
        while (spriteRenderer.color.a > 0.02) {
            Color targetColor = new Color(1, 1, 1, spriteRenderer.color.a);
            targetColor.a = Mathf.Lerp(targetColor.a, 0, 0.05f);
            spriteRenderer.color = targetColor;
            yield return new WaitForSeconds(0.016f);
        }
        gameObject.SetActive(false);
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(DeleteAttacksEvent)) {
            DeleteAttacksEvent deleteEvent = e as DeleteAttacksEvent;
            if (deleteEvent.owner == owner) {
                StartCoroutine(Deactivate());
            }
        }
    }

}
