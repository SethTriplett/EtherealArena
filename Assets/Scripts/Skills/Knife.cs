using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour, IPoolable {

    private enum KnifeState{
        spinning, aiming, released
    }

    private const float baseSpeed = 10f;
    private float speed;
    private float angle;
    private float spinAnimationTimer = 0f;
    private float aimingAnimationTimer = 0f;
    private Quaternion aimedDirection;
    private bool tracking = false;
    private Vector3 target;

    void OnDisable() {
        speed = baseSpeed;
    }

    void Update() {
        if (tracking) {
            float xDiff = target.x - transform.position.x;
            float yDiff = target.y - transform.position.y;
            float aimedAngle = Mathf.Atan2(yDiff, xDiff);
            aimedAngle *= 180 / Mathf.PI;
            aimedDirection = Quaternion.Euler(0f, 0f, aimedAngle);
        }
        if (spinAnimationTimer > 0) {
            gameObject.transform.Rotate(0f, 0f, 1440f * Time.deltaTime);
            spinAnimationTimer -= Time.deltaTime;
        } else if (aimingAnimationTimer > 0) {
            gameObject.transform.rotation = aimedDirection;
            aimingAnimationTimer -= Time.deltaTime;
        } else {
            angle = gameObject.transform.rotation.eulerAngles.z;
            angle *= Mathf.PI / 180;
            gameObject.transform.position += Vector3.right * Mathf.Cos(angle) * speed * Time.deltaTime;
            gameObject.transform.position += Vector3.up * Mathf.Sin(angle) * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (other.gameObject.layer != 8) {
                PlayerStatus playerStatus = other.gameObject.GetComponent<PlayerStatus>();
                if (playerStatus != null) {
                    playerStatus.TakeHit();
                } else {
                    Debug.LogError("No player status script found.");
                }
                speed = 0f;
                gameObject.SetActive(false);
            }
        }
    }

    public void StartSpinningAnimation(float duration) {
        spinAnimationTimer = duration;
        aimedDirection = gameObject.transform.rotation;
    }

    public void StartAimingAnimation(float duration) {
        aimingAnimationTimer = duration;
    }

    public void SetTarget(Vector3 target) {
        this.target = target;
        this.tracking = true;
    }

    public void SetTracking(bool tracking) {
        this.tracking = tracking;
    }

    public void SetAimedAngle(float angle) {
        aimedDirection = Quaternion.Euler(0f, 0f, angle);
    }

}
