using EtherealArena.WorldMap;
using GameJolt.API.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoresManager : MonoBehaviour {

    private enum State {
        NONE,
        READY,
        PLAYING
    }

[SerializeField]
    private Transform listingParent;

    [SerializeField]
    private HighscoresListing prefab;

    [SerializeField]
    private WorldMapPlayer worldMapPlayer;

    private Coroutine moveUp;

    private int currentIndex;

    private State state;

    private Vector3 listingParentStartPosition;

    public static int GetHighscoresIndex(EnemyType enemyType, int level) {
        int index = -1;
        if (enemyType == EnemyType.Dummy && level == 0) {
            return 381299;
        }
        return index;
    }

    private static string ConvertSecondsToMinutesSeconds(int seconds) {
        TimeSpan t = TimeSpan.FromSeconds(seconds);

        string answer = string.Format("{1:D2}:{2:D2}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);
        return answer;
    }

    private void Start() {
        listingParentStartPosition = listingParent.transform.localPosition;
    }

    private void SetHighscores(int index) {
        ResetTable();
        GameJolt.API.Scores.Get(scores => {
            for (int i = 0; i < scores.Length; i++) {
                Score score = scores[i];
                HighscoresListing highscoresListing = Instantiate(prefab);
                highscoresListing.Init(
                    listingParent,
                    score.PlayerName,
                    (i + 1).ToString(),
                    ConvertSecondsToMinutesSeconds(score.Value)
                    );
            }
        }, index, 25, false);
    }

    private IEnumerator MoveUp() {
        listingParent.transform.localPosition = listingParentStartPosition;
        listingParent.gameObject.SetActive(true);
        yield return new WaitForSeconds(5);
        while (true) {
            listingParent.localPosition += new Vector3(0, Time.deltaTime * 140, 0);
            yield return null;
        }
    }

    private void ResetTable() {
        foreach (HighscoresListing highscoresListing in listingParent.GetComponentsInChildren<HighscoresListing>()) {
            Destroy(highscoresListing.gameObject);
        }
        listingParent.gameObject.SetActive(false);
        if (moveUp != null) {
            StopCoroutine(moveUp);
        }
        moveUp = null;
        state = State.NONE;
    }

    private void Update() {
        int highscoresIndex = GetHighscoresIndex(worldMapPlayer.CurrentNode.EnemyType, worldMapPlayer.CurrentNode.EnemyLevel);
        if (worldMapPlayer.CurrentNode.IsBattleNode) {
            if (currentIndex != highscoresIndex) {
                currentIndex = highscoresIndex;
                if (currentIndex > 0) {
                    state = State.READY;
                }
            }
        } else if (currentIndex != highscoresIndex && state == State.PLAYING) {
            ResetTable();
            state = State.NONE;
        }

        bool broughtTableDown = false;

        if (state == State.READY && Input.GetButtonDown("Start") 
            || Input.GetKeyDown(KeyCode.Tab)) {
            SetHighscores(currentIndex);
            moveUp = StartCoroutine(MoveUp());
            state = State.PLAYING;
            broughtTableDown = true;
        }
        
        if (!worldMapPlayer.CurrentNode.IsBattleNode 
            || state == State.PLAYING && Input.GetButtonUp("Start") 
            || Input.GetKeyUp(KeyCode.Tab)
            || (!broughtTableDown && Input.anyKeyDown)) {
            ResetTable();
        }
    }
}
