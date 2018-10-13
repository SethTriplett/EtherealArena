using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOOB : MonoBehaviour {

    private GameObject gameController;
    private OutOfBounds outOfBounds;

    void Start() {
        gameController = GameControllerScript.GetInstance();
        outOfBounds = gameController.GetComponent<OutOfBounds>();
    }

    void Update () {
        if (outOfBounds == null) {
            gameController = GameControllerScript.GetInstance();
            outOfBounds = gameController.GetComponent<OutOfBounds>();
        }
        if (outOfBounds.IsOutOfBounds(gameObject.transform)) {
            gameObject.SetActive(false);
        }
    }

}
