using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeDisplay : MonoBehaviour, IEventListener {

    private GameObject Life1;
    private GameObject Life2;
    private GameObject Life3;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerCurrentHealthEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerCurrentHealthEvent), this);
    }

    void Start() {
        Life1 = transform.Find("Image 1").gameObject;
        Life2 = transform.Find("Image 2").gameObject;
        Life3 = transform.Find("Image 3").gameObject;
    }

    void SetHealthGauge(int health) {
        Life1.SetActive(false);
        Life2.SetActive(false);
        Life3.SetActive(false);
        if (health >= 1) {
            Life1.SetActive(true);
        }
        if (health >= 2) {
            Life2.SetActive(true);
        }
        if (health >= 3) {
            Life3.SetActive(true);
        }
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PlayerCurrentHealthEvent)) {
            PlayerCurrentHealthEvent currentHealthEvent = e as PlayerCurrentHealthEvent;
            SetHealthGauge(currentHealthEvent.currentHealth);
        }
    }

}
