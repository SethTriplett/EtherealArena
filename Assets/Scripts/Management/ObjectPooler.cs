using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler sharedPooler;
    private List<List<GameObject>> pools;
    // For the sake of clear organization, put each pool as a parent object of the pool of objects
    private GameObject[] poolObjects;
    [SerializeField] public GameObject[] danmakuList;
    [SerializeField] private int[] numberOfDanmaku;
    private int[] nextAvailable;

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
        nextAvailable = new int[danmakuList.Length];
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

    // Input a prefab to get its index in list
    public int GetIndex(GameObject prefab) {
        for (int i = 0; i < danmakuList.Length; i++) {
            // Compares the main logic scripts on the objects to check if they're the same
            IPoolable pooledObject = danmakuList[i].GetComponent<IPoolable>();
            IPoolable requestedObject = prefab.GetComponent<IPoolable>();
            if (pooledObject == null) {
                Debug.LogError(danmakuList[i] + " is missing IPoolable.");
            } else if (requestedObject == null) {
                Debug.LogError(prefab + " is missing IPoolable.");
            } else if (pooledObject.GetType() == requestedObject.GetType()) {
                return i;
            }
        }
        // If not found return -1
        return -1;
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
