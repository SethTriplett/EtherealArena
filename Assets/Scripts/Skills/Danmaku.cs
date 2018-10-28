using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danmaku : MonoBehaviour, IPoolable {

    private const float BASE_SPEED = 20f;
    private float speed = BASE_SPEED;
    private float angle;
    private float radAngle;
    private float baseDamage = 1f;
    private GameObject owner;

    void OnDisable() {
        speed = BASE_SPEED;
    }

    void Update() {
        SetAnglesFromRotation();
        gameObject.transform.position += Vector3.right * Mathf.Cos(radAngle) * speed * Time.deltaTime;
        gameObject.transform.position += Vector3.up * Mathf.Sin(radAngle) * speed * Time.deltaTime;
    }

    void SetAnglesFromRotation() {
        angle = transform.rotation.eulerAngles.z;
        radAngle = angle * Mathf.PI / 180f;
    }

    void OnTriggerEnter2D(Collider2D other) {
        // if colliding with an enemy, deal damage play hit animation
        if (other.CompareTag("Enemy")) {
            EnemyStatus enemyStatus = other.GetComponent<EnemyStatus>();
            if (enemyStatus != null) {
                enemyStatus.TakeDamage(baseDamage);
            }
            if (owner != null) {
                PlayerStatus playerStatusReference = owner.GetComponent<PlayerStatus>();
                if (playerStatusReference != null) playerStatusReference.gainEnergy(1f);
            }
            speed = 0f;
            gameObject.SetActive(false);
        }
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

}
