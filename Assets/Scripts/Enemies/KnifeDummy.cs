using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDummy : MonoBehaviour {

    private ObjectPooler knifePooler;
    [SerializeField] private Transform playerTransform;
    private float attackingTimer = 0f;

    void Start() {
        knifePooler = ObjectPooler.sharedPooler;
    }

    void Update() {
        if (attackingTimer <= 0) {
            StartCoroutine(JackTheRipper(new Vector3(0, 0, 0), 5f, 60));
            attackingTimer = 30f;
        } else {
            attackingTimer -= Time.deltaTime;
        }
    }

    void KnifeToss() {

    }

    IEnumerator JackTheRipper(Vector3 centerPoint, float radius, int number) {
        for (int i = 0; i < number; i++) {
            float angle = (-i / (float) number) * 2 * Mathf.PI + Mathf.PI / 2;
            float xPos = centerPoint.x + radius * Mathf.Cos(angle);
            float yPos = centerPoint.y + radius * Mathf.Sin(angle);
            GameObject knife = knifePooler.GetDanmaku(1);
            if (knife != null) {
                Knife knifeScript = knife.GetComponent<Knife>();
                knife.transform.position = new Vector3(xPos, yPos, 0f);
                knife.SetActive(true);
                knifeScript.SetTarget(playerTransform);
                knifeScript.StartSpinningAnimation(3f);
                knifeScript.StartAimingAnimation(2.5f);
                knifeScript.SetTracking(true);
            }
            yield return new WaitForSeconds(2f/number);
        }
    }

}
