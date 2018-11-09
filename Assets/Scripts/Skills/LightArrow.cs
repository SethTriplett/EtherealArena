using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightArrow : MonoBehaviour, IPoolable, IEventListener {

    private enum ArrowState {
        charging, released
    }

    private ArrowState arrowState;
    private float speed;
    private float angle;
    private int chargeLevel;
    private GameObject owner;

    void Awake() {
        speed = 30f;
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(DeleteAttacksEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(DeleteAttacksEvent), this);
    }

    void Update() {
        if (arrowState == ArrowState.charging) {
            angle = transform.rotation.eulerAngles.z;
        } else if (arrowState == ArrowState.released) {
            transform.position += new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * Time.deltaTime * speed, Mathf.Sin(Mathf.Deg2Rad * angle) * Time.deltaTime * speed, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // if colliding with an enemy, deal damage play hit animation
        if (other.CompareTag("Enemy")) {
            EnemyStatus enemyStatus = other.GetComponent<EnemyStatus>();
            float damage = CalcDamage(chargeLevel);
            if (enemyStatus != null) {
                enemyStatus.TakeDamage(damage);
            } else {
                Debug.LogError("Enemy Status not found.");
            }
            if (owner != null) {
                PlayerStatus playerStatusReference = owner.GetComponent<PlayerStatus>();
                if (playerStatusReference != null) playerStatusReference.gainEnergy(damage);
            }
            speed = 0f;
            gameObject.SetActive(false);
        }
    }

    public void SetCharging() {
        arrowState = ArrowState.charging;
    }

    public void SetReleased() {
        arrowState = ArrowState.released;
        SetSpeed(chargeLevel);
    }
    
    public void SetChargeLevel(int chargeLevel) {
        this.chargeLevel = chargeLevel;
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }

    private void SetSpeed(int chargeLevel) {
        switch(chargeLevel) {
            case 0:
                this.speed = 5f;
                break;
            case 1:
                this.speed = 10f;
                break;
            case 2:
                this.speed = 15f;
                break;
            case 3:
                this.speed = 20f;
                break;
            case 4:
                this.speed = 25f;
                break;
            case 5:
                this.speed = 30f;
                break;
         }
    }

    private float CalcDamage(int chargeLevel) {
        switch(chargeLevel) {
            case 0:
                return 0.1f;
            case 1:
                return 0.5f;
            case 2:
                return 1f;
            case 3:
                return 3f;
            case 4:
                return 5f;
            case 5:
                return 8f;
        }
        return 0.1f;
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
