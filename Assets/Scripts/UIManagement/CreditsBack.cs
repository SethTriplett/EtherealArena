using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsBack : MonoBehaviour {

    [SerializeField] private TitleScreen titleScreen;

    void Update() {
        if (titleScreen.creditsOn) {
            if (Input.GetButtonDown("B")) {
                titleScreen.creditsOn = false;
            }
        }
    }
}
