using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler sharedPooler;
    private List<List<GameObject>> pools;
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
        pools = new List<List<GameObject>>();
        for (int y = 0; y < danmakuList.Length; y++) {
            List<GameObject> pool = new List<GameObject>();
            for (int x = 0; x < numberOfDanmaku[y]; x++) {
                GameObject singleDanmaku = (GameObject) Instantiate(danmakuList[y]);
                singleDanmaku.SetActive(false);
                pool.Add(singleDanmaku);
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
