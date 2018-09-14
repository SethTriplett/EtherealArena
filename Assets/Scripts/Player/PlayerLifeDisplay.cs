using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLifeDisplay : MonoBehaviour {

    private int health;
    private GameObject Life1;
    private GameObject Life2;
    private GameObject Life3;
    [SerializeField] private PlayerStatus playerStatus;

    void Start() {
        Life1 = transform.Find("Image 1").gameObject;
        Life2 = transform.Find("Image 2").gameObject;
        Life3 = transform.Find("Image 3").gameObject;
    }

    void Update() {
        this.health = playerStatus.GetHealth();
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

}
