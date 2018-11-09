using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkArrow : MonoBehaviour, IPoolable, IEventListener {

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

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(DeleteAttacksEvent)) {
            DeleteAttacksEvent deleteAttacksEvent = e as DeleteAttacksEvent;
            if (deleteAttacksEvent.owner == owner) {
                Deactivate();
            }
        }
    }
 
}
