using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Danmaku : MonoBehaviour {

    private const float BASE_SPEED = 20f;
    private float speed = BASE_SPEED;
    private float angle;
    private CircleCollider2D hitbox;
    private float baseDamage = 1f;
    private int userAttack = 0;
    private PlayerControl playerControlReference;

    void Start() {
        hitbox = GetComponent<CircleCollider2D>();
    }

    void OnDisable() {
        speed = BASE_SPEED;
    }

    void Update() {
        angle = gameObject.transform.rotation.eulerAngles.z;
        angle *= Mathf.PI / 180;
        gameObject.transform.position += Vector3.right * Mathf.Cos(angle) * speed * Time.deltaTime;
        gameObject.transform.position += Vector3.up * Mathf.Sin(angle) * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        // if colliding with an enemy, deal damage play hit animation
        if (other.CompareTag("Enemy")) {
            EnemyStatus enemyStatus = other.GetComponent<EnemyStatus>();
            if (enemyStatus != null) {
                float damage;
                if (userAttack >= 0) {
                    damage = baseDamage * (1 + (userAttack / 100f));
                } else {
                    // for now scale negative attack downward disproportionately
                    //float damage = baseDamage / (1 + (- userAttack / 100f));

                    damage = baseDamage * (1 + (userAttack / 100f));
                }
                enemyStatus.TakeDamage(damage);
            }
            if (playerControlReference != null) {
                playerControlReference.gainEnergy(1f);
            }
            speed = 0f;
            gameObject.SetActive(false);
        }
    }

    public void setPlayerControlReference(PlayerControl pcr) {
        this.playerControlReference = pcr;
    }

}
