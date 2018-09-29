using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerLoader : MonoBehaviour {
    // Loads a game controller when the scene starts

    private static GameObject gameController;
    [SerializeField] private GameObject prefab;

    void Start() {
        if (gameController == null) {
            gameController = Instantiate(prefab);
        }
    }
}
