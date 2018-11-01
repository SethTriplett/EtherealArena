using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseFlamethrower : MonoBehaviour, ISkill {

    // cooldown timer in frames
    private float readyTimer;
    private ObjectPooler danmakuPool;
    [SerializeField] private GameObject fireballPrefab;
    private int fireballIndex;

    void Start () {
        readyTimer = 0f;
        danmakuPool = ObjectPooler.instance;
        fireballIndex = danmakuPool.GetIndex(fireballPrefab);
        if (fireballIndex == -1) {
            Debug.LogError("Fireball index not found in pooler.");
            fireballIndex = 0;
        }
    }

    void Update () {
        if (readyTimer > 0) {
            readyTimer -= Time.deltaTime;
        }
    }

    public void UseSkill(Transform hand, bool isAutoRelease) {
        if (readyTimer <= 0) {
            GameObject nextFireball = danmakuPool.GetDanmaku(fireballIndex);
            Flamethrower flamethrowerScript = nextFireball.GetComponent<Flamethrower>();
            if (nextFireball != null) {
                nextFireball.transform.position = hand.position;
                nextFireball.transform.rotation = hand.rotation;
                flamethrowerScript.SetOwner(gameObject);
                nextFireball.SetActive(true);
                readyTimer = 0.1f;
            }
        }
    }

}
