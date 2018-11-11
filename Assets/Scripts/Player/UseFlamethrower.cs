using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseFlamethrower : MonoBehaviour, ISkill {

    // cooldown timer in frames
    private float readyTimer;
    private ObjectPooler danmakuPool;
    [SerializeField] private GameObject fireballPrefab;
    private int fireballIndex;
    private bool usingFire;

    void Start () {
        readyTimer = 0f;
        danmakuPool = ObjectPooler.instance;
        fireballIndex = danmakuPool.GetIndex(fireballPrefab);
        if (fireballIndex == -1) {
            Debug.LogError("Fireball index not found in pooler.");
            fireballIndex = 0;
        }
        usingFire = false;
    }

    void Update () {
        if (readyTimer > 0) {
            readyTimer -= Time.deltaTime;
        }
    }

    public void UseSkill(Transform hand) {
        if (!usingFire) {
            AudioManager.GetInstance().PlaySound(Sound.FireLoop);
            usingFire = true;
        }
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

    public void ReleaseSkill() {
        usingFire = false;
        AudioManager.GetInstance().StopSound(Sound.FireStart);
        AudioManager.GetInstance().StopSound(Sound.FireLoop);
        AudioManager.GetInstance().PlaySound(Sound.FireEnd);
    }

    public void AimSkill(Transform hand) {
        // Do nothing
    }

    public void SetActiveSkill() {
        // Do nothing
    }

    public void SetInactiveSkill() {
        usingFire = false;
        AudioManager.GetInstance().StopSound(Sound.FireStart);
        AudioManager.GetInstance().StopSound(Sound.FireLoop);
        AudioManager.GetInstance().PlaySound(Sound.FireEnd);
    }

}
