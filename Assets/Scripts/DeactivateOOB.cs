using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOOB : MonoBehaviour {

    public GameObject gameController;
    private OutOfBounds outOfBounds;

    void Start() {
        outOfBounds = gameController.GetComponent<OutOfBounds>();
    }

    void Update () {
        if (outOfBounds.IsOutOfBounds(gameObject.transform)) {
            gameObject.SetActive(false);
        }
    }
}
