using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {

    public static GameObject instance;

    void Awake() {
        if (instance == null) {
            instance = gameObject;
            //DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
