using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDummy : MonoBehaviour, IEventListener {

    private ObjectPooler knifePooler;
    [SerializeField] private Transform playerTransform;
    private float attackingTimer = 0f;
    private bool stopAttacking = false;
    private bool secondForm;

    [SerializeField] private GameObject knifePrefab;
    private int knifeIndex;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerDefeatEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerDefeatEvent), this);
    }

     void Start() {
        knifePooler = ObjectPooler.instance;
        knifeIndex = knifePooler.GetIndex(knifePrefab);
        if (knifeIndex == -1) {
            Debug.LogError("Knife index not found in pooler");
            knifeIndex = 0;
        }
    }

    void Update() {
        if (!stopAttacking) {
            Attack();
        }
    }
    
    void Attack() {
        attackingTimer -= Time.deltaTime;
        if (!secondForm) {
            if (attackingTimer <= 0) {
                StartCoroutine(KnifeToss(5));
            }
            attackingTimer = 10f;
        } else {
            if (attackingTimer <= 0) {
                int attack = (int) Mathf.Floor(Random.Range(0, 2));
                if (attack == 0) {
                    StartCoroutine(KnifeToss(7));
                    attackingTimer = 5f;
                } else {
                    StartCoroutine(JackTheRipper(new Vector3(0, 0, 0), 5f, 60));
                    attackingTimer = 10f;
                }
            }
        }
    }

    IEnumerator KnifeToss(int number) {
        Vector3 targetVector = playerTransform.position - transform.position;
        targetVector.Normalize();
        Vector3 perpendicularVector = new Vector3(-targetVector.y, targetVector.x, 0f);
        Vector3[] knifePos = new Vector3[number];
        GameObject[] knives = new GameObject[number];
        // Create a number of knives that fan out.
        for (int i = 0; i < number; i++) {
            float adjustedIndex = i - ((number - 1) / 2f);
            knifePos[i] = adjustedIndex * perpendicularVector + targetVector + transform.position;
            GameObject knife = knifePooler.GetDanmaku(knifeIndex);
            knife.SetActive(true);
            if (knife != null) {
                Knife knifeScript = knife.GetComponent<Knife>();
                knife.transform.position = gameObject.transform.position;
                knifeScript.SetTarget(playerTransform.position);
                knifeScript.StartSpinningAnimation(2f);
                knifeScript.StartAimingAnimation(1f);
                knives[i] = knife;
            }
        }
        // Lerp to be in front of the user
        for (int j = 0; j < 60; j++) {
            for (int i = 0; i < number; i++) {
                GameObject knife = knives[i];
                knife.transform.position = Vector3.LerpUnclamped(knife.transform.position, knifePos[i], 0.05f);
            }
            yield return new WaitForSeconds(0.016f);
        }
        for (int i = 0; i < number; i++) {
            GameObject knife = knives[i];
            knife.transform.position = knifePos[i];
        }
    }

    IEnumerator JackTheRipper(Vector3 centerPoint, float radius, int number) {
        for (int i = 0; i < number; i++) {
            float angle = (-i / (float) number) * 2 * Mathf.PI + Mathf.PI / 2;
            float xPos = centerPoint.x + radius * Mathf.Cos(angle);
            float yPos = centerPoint.y + radius * Mathf.Sin(angle);
            GameObject knife = knifePooler.GetDanmaku(knifeIndex);
            if (knife != null) {
                Knife knifeScript = knife.GetComponent<Knife>();
                knife.transform.position = new Vector3(xPos, yPos, 0f);
                knife.SetActive(true);
                knifeScript.SetTarget(playerTransform.position);
                knifeScript.StartSpinningAnimation(3f);
                knifeScript.StartAimingAnimation(3f);
            }
            yield return new WaitForSeconds(2f/number);
        }
    }

    void StopAttacking() {
        stopAttacking = true;
        StopAllCoroutines();
    }

    public void SetSecondForm(bool secondForm) {
        this.secondForm = secondForm;
    }

    public void SetPlayerTransform(Transform player) {
        this.playerTransform = player;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PlayerVictoryEvent)) {
            StopAttacking();
            transform.Rotate(0f, 0f, 90f);
        } else if (e.GetType() == typeof(PlayerDefeatEvent)) {
            StopAttacking();
        }
     }

}
