using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingFinish : MonoBehaviour {

    private float delayTimer;

    void Awake() {
        delayTimer = 3f;
    }

    void Update() {
        if (delayTimer > 0f) {
            delayTimer -= Time.deltaTime;
        } else {
            if (Input.GetButtonUp("A")
                || Input.GetButtonUp("B")
                || Input.GetButtonUp("Start")
                || Input.GetButtonUp("Select")
                || Input.GetButtonUp("X")
                || Input.GetButtonUp("Y")
                || Input.GetButtonUp("RB")
                || Input.GetButtonUp("LB")
                ) {
                Vignette.LoadScene("TitleScreen");
            }
        }
    }
}
