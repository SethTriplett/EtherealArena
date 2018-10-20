using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseDanmaku : MonoBehaviour, ISkill {

    // cooldown timer in frames
    private float readyTimer;
    private ObjectPooler danmakuPool;
    [SerializeField] private GameObject danmakuPrefab;
    private int danmakuIndex;

    void Start () {
        readyTimer = 0f;
        danmakuPool = ObjectPooler.sharedPooler;
        danmakuIndex = danmakuPool.GetIndex(danmakuPrefab);
        if (danmakuIndex == -1) {
            Debug.LogError("Danmaku index not found in pooler.");
            danmakuIndex = 0;
        }
    }

    void Update () {
        if (readyTimer > 0) {
            readyTimer -= Time.deltaTime;
        }
    }

    public void UseSkill(Transform hand, bool isAutoRelease) {
        if (readyTimer <= 0) {
            GameObject nextDanmaku = danmakuPool.GetDanmaku(danmakuIndex);
            Danmaku danmakuScript = nextDanmaku.GetComponent<Danmaku>();
            if (nextDanmaku != null) {
                nextDanmaku.transform.position = hand.position;
                nextDanmaku.transform.rotation = hand.rotation;
                danmakuScript.SetPlayerStatusReference(gameObject.GetComponent<PlayerStatus>());
                nextDanmaku.SetActive(true);
                readyTimer = 0.166667f;
            }
        }
    }

}
