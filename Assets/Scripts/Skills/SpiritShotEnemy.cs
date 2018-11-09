using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritShotEnemy : MonoBehaviour, IPoolable, IEventListener {

    private const float BASE_SPEED = 10f;
    private float speed = BASE_SPEED;
    private float angle;
    private float radAngle;
    private GameObject owner;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(DeleteAttacksEvent), this);
    }

    void OnDisable() {
        speed = BASE_SPEED;
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(DeleteAttacksEvent), this);
    }

    void Update() {
        gameObject.transform.position += Vector3.right * Mathf.Cos(radAngle) * speed * Time.deltaTime;
        gameObject.transform.position += Vector3.up * Mathf.Sin(radAngle) * speed * Time.deltaTime;
    }

    void SetAnglesFromRotation() {
        angle = transform.rotation.eulerAngles.z;
        radAngle = angle * Mathf.PI / 180f;
    }

    public void SetAngles(float angle) {
        this.angle = angle;
        radAngle = angle * Mathf.PI / 180f;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && other.gameObject.layer != 8) {
            PlayerStatus playerStatus = other.GetComponent<PlayerStatus>();
            if (playerStatus != null) {
                playerStatus.TakeHit();
            } else {
                Debug.LogError("Player status not found.");
            }
            speed = 0f;
            Deactivate();
        }
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(DeleteAttacksEvent)) {
            DeleteAttacksEvent deleteAttacksEvent = e as DeleteAttacksEvent;
            if (deleteAttacksEvent.owner == owner) {
                Deactivate();
            }
        }
    }

}
