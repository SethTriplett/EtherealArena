using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseDanmaku : MonoBehaviour, ISkill {

    // cooldown timer in frames
    private float readyTimer;
    private ObjectPooler danmakuPool;

    void Start () {
        readyTimer = 0f;
        danmakuPool = ObjectPooler.sharedPooler;
    }

    void Update () {
        if (readyTimer > 0) {
            readyTimer -= Time.deltaTime;
        }
    }

    public void UseSkill(Transform hand, bool isAutoRelease) {
        if (readyTimer <= 0) {
            GameObject nextDanmaku = danmakuPool.GetDanmaku(0);
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
