using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Persists the game controller through scenes as a singleton
public class GameControllerScript : MonoBehaviour {

    public static GameObject instance;

    void Awake() {
        if (instance == null) {
            instance = FindObjectOfType<GameControllerScript>().gameObject;
            if (instance == null) {
                instance = gameObject;
                DontDestroyOnLoad(gameObject);
            } else {
                DontDestroyOnLoad(instance);
                if (instance != this.gameObject) Destroy(this.gameObject);
            }
        } else {
            Destroy(gameObject);
        }
    }

    public static GameObject GetInstance() {
        if (instance != null) {
            return instance;
        } else {
            GameControllerScript gc = FindObjectOfType<GameControllerScript>();
            if (gc != null) instance = gc.gameObject;
            if (instance != null) {
                DontDestroyOnLoad(instance);
                return instance;
            } else {
                Debug.LogError("Returning null GameController");
                return null;
            }
        }
    }

}
