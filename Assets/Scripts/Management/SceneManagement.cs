using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private GameObject defeatScreen;
    private bool victory = false;
    private bool defeat = false;

    public void PlayerVictory() {
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        playerControl.StopMovement();
        player.layer = 8;
        victoryScreen.SetActive(true);
        victory = true;
    }

    public void OpponentVictory() {
        PlayerControl playerControl = player.GetComponent<PlayerControl>();
        playerControl.StopMovement();
        player.layer = 8;
        defeatScreen.SetActive(true);
        defeat = true;
    }

    void Update() {
        if (victory || defeat) {
            if (Input.GetAxis("A") > 0) {
                SceneManager.LoadScene("WorldMap");
            }
        }
    }
}
