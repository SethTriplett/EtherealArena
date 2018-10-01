using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Holds information for the battle to load when enter from the world map
public class LoadBattleScene : MonoBehaviour {
    
    private EnemyType enemyType;
    private int enemyLevel;
    // private int backgroundEnum
    private SceneManagement sceneManagement;
    private ObjectPooler objectPooler;

    public GameObject[] enemyPrefabs;

    void Start() {
        sceneManagement = GetComponent<SceneManagement>();
        objectPooler = GetComponent<ObjectPooler>();
        SceneManager.sceneLoaded += PrepareGameController;
    }

    public void PrepareGameController(Scene scene, LoadSceneMode mode) {
        if (scene.name.Equals("Battle")) {
            sceneManagement.Reset();
            objectPooler.ReinstantiatePools();
            LoadEnemy();
        }
    }

    void LoadEnemy() {
        GameObject enemy;
        if ((int) enemyType > enemyPrefabs.Length) {
            enemy = Instantiate(enemyPrefabs[0]);
        } else {
            enemy = Instantiate(enemyPrefabs[(int) enemyType]);
        }
        EnemyStatus enemyStatus = enemy.GetComponent<EnemyStatus>();
        enemyStatus.maxHealth = 5 * (enemyLevel + 1);
    }

}