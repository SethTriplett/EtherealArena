﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour, IPoolable, IEventListener {

    private enum KnifeState{
        spinning, aiming, released
    }

    private const float baseSpeed = 10f;
    private float speed;
    private float angle;
    private float spinAnimationTimer = 0f;
    private float aimingAnimationTimer = 0f;
    private Quaternion aimedDirection;
    private bool tracking = true;
    private Transform target;
    private GameObject owner;

    private bool spinning;
    private bool aiming;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(DeleteAttacksEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(DeleteAttacksEvent), this);
        speed = baseSpeed;
    }

    void Update() {
        if (tracking) {
            float xDiff = target.position.x - transform.position.x;
            float yDiff = target.position.y - transform.position.y;
            float aimedAngle = Mathf.Atan2(yDiff, xDiff);
            aimedAngle *= 180 / Mathf.PI;
            aimedDirection = Quaternion.Euler(0f, 0f, aimedAngle);
        }
        if (spinAnimationTimer > 0) {
            if (!spinning) {
                spinning = true;
                AudioManager.GetInstance().PlaySound(Sound.Whirl);
            }
            gameObject.transform.Rotate(0f, 0f, 1440f * Time.deltaTime);
            spinAnimationTimer -= Time.deltaTime;
        } else if (spinAnimationTimer <= 0 && spinning) {
            AudioManager.GetInstance().StopSound(Sound.Whirl);
            AudioManager.GetInstance().PlaySound(Sound.KnifeDraw);
            spinning = false;
            aiming = true;
        } else if (aimingAnimationTimer > 0) {
            gameObject.transform.rotation = aimedDirection;
            aimingAnimationTimer -= Time.deltaTime;
        } else {
            if (aiming == true) {
                aiming = false;
                AudioManager.GetInstance().PlaySound(Sound.WhooshSmall);
            }
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

    private void Deactivate() {
        gameObject.SetActive(false);
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    public void StartSpinningAnimation(float duration) {
        spinAnimationTimer = duration;
        aimedDirection = gameObject.transform.rotation;
    }

    public void StartAimingAnimation(float duration) {
        aimingAnimationTimer = duration;
    }

    public void SetTarget(Transform target) {
        this.target = target;
        this.tracking = true;
    }

    public void SetTracking(bool tracking) {
        this.tracking = tracking;
    }

    public void SetAimedAngle(float angle) {
        aimedDirection = Quaternion.Euler(0f, 0f, angle);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(DeleteAttacksEvent)) {
            DeleteAttacksEvent deleteAttacksEvent = e as DeleteAttacksEvent;
            if (deleteAttacksEvent.owner == this.owner) Deactivate();
        }
    }

}
