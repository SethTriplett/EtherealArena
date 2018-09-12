using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler sharedPooler;
    private List<GameObject> pool;
    public GameObject danmaku;
    public int numberOfDanmaku;
    private int nextAvailable;

    void Awake() {
        if (sharedPooler == null) {
            sharedPooler = this;
        } else if (sharedPooler != this) {
            Destroy(gameObject);
        }
    }

    void Start () {
        pool = new List<GameObject>();
        for (int x = 0; x < numberOfDanmaku; x++) {
            GameObject singleDanmaku = (GameObject) Instantiate(danmaku);
            singleDanmaku.SetActive(false);
            pool.Add(singleDanmaku);
        }
        nextAvailable = 0;
    }

    public GameObject GetDanmaku() {
        int startingIndex = nextAvailable;
        do {
            if (!pool[nextAvailable].activeInHierarchy) {
                int readyIndex = nextAvailable;
                nextAvailable++;
                nextAvailable %= numberOfDanmaku;
                return pool[readyIndex];
            } else {
                nextAvailable++;
                nextAvailable %= numberOfDanmaku;
            }
        } while (nextAvailable != startingIndex);
        return null;
    }
}
