using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Holds information for the battle to load when enter from the world map
public class LoadBattleSceneScript : MonoBehaviour {
    
    private EnemyType enemyType;
    private int enemyLevel;
    private int enemyMaxPhase;
    // private int backgroundEnum
    private SceneManagement sceneManagement;
    private ObjectPooler objectPooler;

    public GameObject[] enemyPrefabs;

    public EnemyType EnemyType {
        get {
            return enemyType;
        }

        set {
            enemyType = value;
        }
    }

    public int EnemyLevel {
        get {
            return enemyLevel;
        }

        set {
            enemyLevel = value;
        }
    }

    public int EnemyMaxPhase {
        get {
            return enemyMaxPhase;
        }

        set {
            enemyLevel = value;
        }
    }


    void Start() {
        sceneManagement = GetComponent<SceneManagement>();
        objectPooler = GetComponent<ObjectPooler>();
        SceneManager.sceneLoaded += PrepareGameController;
    }

    void PrepareGameController(Scene scene, LoadSceneMode mode) {
        if (scene.name.Equals("Battle")) {
            sceneManagement.Reset();
            objectPooler.ReinstantiatePools();
            LoadEnemy();
        }
    }

    void LoadEnemy() {
        if ((int) enemyType > enemyPrefabs.Length) {
            Instantiate(enemyPrefabs[0]);
        } else {
            Instantiate(enemyPrefabs[(int) enemyType]);
        }
        EventMessanger.GetInstance().TriggerEvent(new EnemyStartingDataEvent(enemyLevel, enemyMaxPhase));
    }

    public void LoadBattleScene(EnemyType enemyType, int enemyLevel, int enemyMaxPhase) {
        this.enemyType = enemyType;
        this.enemyLevel = enemyLevel;
        this.enemyMaxPhase = enemyMaxPhase;
        SceneManager.LoadScene("Battle");
    }

}