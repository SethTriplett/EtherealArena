using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler sharedPooler;
    private List<List<GameObject>> pools;
    // For the sake of clear organization, put each pool as a parent object of the pool of objects
    private GameObject[] poolObjects;
    public GameObject[] danmakuList = new GameObject[2];
    public int[] numberOfDanmaku = new int[2];
    private int[] nextAvailable = new int[2];

    void Awake() {
        if (sharedPooler == null) {
            sharedPooler = this;
        } else if (sharedPooler != this) {
            Destroy(gameObject);
        }
    }

    void Start () {
        ReinstantiatePools();
    }

    public void ReinstantiatePools() {
        poolObjects = new GameObject[danmakuList.Length];
        for (int y = 0; y < danmakuList.Length; y++) {
            GameObject poolObject = new GameObject();
            poolObject.name = danmakuList[y].name + " Pool";
            poolObjects[y] = poolObject;
        }
        pools = new List<List<GameObject>>();
        for (int y = 0; y < danmakuList.Length; y++) {
            List<GameObject> pool = new List<GameObject>();
            for (int x = 0; x < numberOfDanmaku[y]; x++) {
                GameObject singleDanmaku = (GameObject) Instantiate(danmakuList[y]);
                singleDanmaku.SetActive(false);
                pool.Add(singleDanmaku);
                singleDanmaku.transform.SetParent(poolObjects[y].transform);
            }
            pools.Add(pool);
        }
        for (int x = 0; x < nextAvailable.Length; x++) {
            nextAvailable[x] = 0;
        }
    }

    public GameObject GetDanmaku(int type) {
        int startingIndex = nextAvailable[type];
        do {
            if (!pools[type][nextAvailable[type]].activeInHierarchy) {
                int readyIndex = nextAvailable[type];
                nextAvailable[type]++;
                nextAvailable[type] %= numberOfDanmaku[type];
                return pools[type][readyIndex];
            } else {
                nextAvailable[type]++;
                nextAvailable[type] %= numberOfDanmaku[type];
            }
        } while (nextAvailable[type] != startingIndex);
        return null;
    }
}
