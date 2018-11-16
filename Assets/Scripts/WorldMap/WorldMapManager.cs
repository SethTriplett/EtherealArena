using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EtherealArena.WorldMap {

    public class WorldMapManager : MonoBehaviour {
    
        [SerializeField] List<WorldMapEdge> edges;
        [SerializeField] List<WorldMapNode> nodes;

        void Start() {
            AudioManager.GetInstance().StopMusic(Soundtrack.TutorialTheme);
            AudioManager.GetInstance().StopMusic(Soundtrack.VampireTheme);
            AudioManager.GetInstance().StopMusic(Soundtrack.PsychicTheme);
            AudioManager.GetInstance().StopMusic(Soundtrack.FinalBossTheme);
            AudioManager.GetInstance().StopMusic(Soundtrack.EndingTheme);

            AudioManager.GetInstance().StartMusic(Soundtrack.TitleTheme);
            LoadMapState();
        }

        void Update() {
            if (Input.GetKey(KeyCode.X)
                && (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))
                && (Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Keypad0))) {
                    GameStateHandler gameStateHandler = GameControllerScript.GetInstance().GetComponent<GameStateHandler>();
                    for (int x = 0; x < 4; x++) {
                        gameStateHandler.SetBossDefeated(EnemyType.DarkPlayer, x);
                        gameStateHandler.SetBossDefeated(EnemyType.Dummy, x);
                        gameStateHandler.SetBossDefeated(EnemyType.Vampire, x);
                        gameStateHandler.SetBossDefeated(EnemyType.Psychic, x);
                    }
                    LoadMapState();
            }
        }

        public void LoadMapState() {
            GameStateHandler gameStateHandler = GameControllerScript.GetInstance().GetComponent<GameStateHandler>();
            WorldMapNode currentNode = nodes[gameStateHandler.GetCurrentNode()];
            if (currentNode != null) {
                GameObject player = GameObject.FindGameObjectWithTag("WorldMapPlayer");
                player.transform.position = currentNode.transform.position;
            }

            // Enable/Disable edges
            if (gameStateHandler.GetBossDefeated(EnemyType.Dummy, 2)) {
                edges[0].SetCanCross(true);
                edges[0].gameObject.SetActive(true);
                edges[1].SetCanCross(true);
                edges[1].gameObject.SetActive(true);
                edges[2].SetCanCross(true);
                edges[2].gameObject.SetActive(true);
            } else {
                edges[0].SetCanCross(false);
                edges[0].gameObject.SetActive(false);
                edges[1].SetCanCross(false);
                edges[1].gameObject.SetActive(false);
                edges[2].SetCanCross(false);
                edges[2].gameObject.SetActive(false);
            }
            if (gameStateHandler.GetBossDefeated(EnemyType.Vampire, 1)
                && gameStateHandler.GetBossDefeated(EnemyType.Psychic, 1)) {
                edges[3].SetCanCross(true);
                edges[3].gameObject.SetActive(true);
                edges[4].SetCanCross(true);
                edges[4].gameObject.SetActive(true);
                edges[5].SetCanCross(true);
                edges[5].gameObject.SetActive(true);
            } else {
                edges[3].SetCanCross(false);
                edges[3].gameObject.SetActive(false);
                edges[4].SetCanCross(false);
                edges[4].gameObject.SetActive(false);
                edges[5].SetCanCross(false);
                edges[5].gameObject.SetActive(false);
            }
            if (gameStateHandler.GetBossDefeated(EnemyType.Vampire, 2)) {
                edges[6].SetCanCross(true);
                edges[6].gameObject.SetActive(true);
            } else {
                edges[6].SetCanCross(false);
                edges[6].gameObject.SetActive(false);
            }
            if (gameStateHandler.GetBossDefeated(EnemyType.Psychic, 2)) {
                edges[7].SetCanCross(true);
                edges[7].gameObject.SetActive(true);
                edges[8].SetCanCross(true);
                edges[8].gameObject.SetActive(true);
            } else {
                edges[7].SetCanCross(false);
                edges[7].gameObject.SetActive(false);
                edges[8].SetCanCross(false);
                edges[8].gameObject.SetActive(false);
            }
            if (gameStateHandler.GetBossDefeated(EnemyType.Vampire, 3)
                && gameStateHandler.GetBossDefeated(EnemyType.Psychic, 3)) {
                edges[9].SetCanCross(true);
                edges[9].gameObject.SetActive(true);
                edges[10].SetCanCross(true);
                edges[10].gameObject.SetActive(true);
                edges[11].SetCanCross(true);
                edges[11].gameObject.SetActive(true);
            } else {
                edges[9].SetCanCross(false);
                edges[9].gameObject.SetActive(false);
                edges[10].SetCanCross(false);
                edges[10].gameObject.SetActive(false);
                edges[11].SetCanCross(false);
                edges[11].gameObject.SetActive(false);
            }
        }

        public void ConsumeEvent(IEvent e) {
            GameStateHandler gameStateHandler = GameControllerScript.GetInstance().GetComponent<GameStateHandler>();
            if (e.GetType() == typeof(WorldMapMovementEvent)) {
                WorldMapMovementEvent movementEvent = e as WorldMapMovementEvent;
                for (int x = 0; x < nodes.Count; x++) {
                    if (movementEvent.currentNode.Equals(nodes[x])) {
                        gameStateHandler.SetCurrentNode(x);
                        return;
                    }
                }
            }
        }

    }



}