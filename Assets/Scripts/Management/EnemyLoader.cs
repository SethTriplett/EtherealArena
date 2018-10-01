using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoader : MonoBehaviour {

    public GameObject[] enemyPrefabs;

    public void LoadEnemy(EnemyType enemyType, int enemyLevel) {
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
