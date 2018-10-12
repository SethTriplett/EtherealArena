using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeDummy : MonoBehaviour {

    private ObjectPooler knifePooler;
    [SerializeField] private Transform playerTransform;
    private float attackingTimer = 0f;
    private bool stopAttacking = false;
    [SerializeField] private bool secondForm;

    void Start() {
        knifePooler = ObjectPooler.sharedPooler;
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
                StartCoroutine(CircleHell(transform.position, 5, 1, 8, playerTransform.position));
                attackingTimer = 10f;
            }
            
        } else {
            if (attackingTimer <= 0) {
                //CircleHell(transform.position, 6, 5);
                int attack = (int) Mathf.Floor(Random.Range(0, 2));
                if (attack == 0) {
                    StartCoroutine(CircleHell(transform.position, 6, 5, 3, playerTransform.position));
                    attackingTimer = 5f;
                } else {
                    StartCoroutine(CircleHell(transform.position, 6, 5, 3, playerTransform.position));
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
            GameObject knife = knifePooler.GetDanmaku(1);
            knife.SetActive(true);
            if (knife != null) {
                Knife knifeScript = knife.GetComponent<Knife>();
                knife.transform.position = gameObject.transform.position;
                knifeScript.SetTarget(playerTransform);
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
            GameObject knife = knifePooler.GetDanmaku(1);
            if (knife != null) {
                Knife knifeScript = knife.GetComponent<Knife>();
                knife.transform.position = new Vector3(xPos, yPos, 0f);
                knife.SetActive(true);
                knifeScript.SetTarget(playerTransform);
                knifeScript.StartSpinningAnimation(3f);
                knifeScript.StartAimingAnimation(3f);
            }
            yield return new WaitForSeconds(2f/number);
        }
    }


    public void StopAttacking() {
        stopAttacking = true;
        StopAllCoroutines();
    }

    IEnumerator CircleHell(Vector3 centerPoint, int number, int radius, int numOfCircles, Vector3 playerLoc)
    {
        for (int j = 0; j < numOfCircles; j++)
        {
            float angleTar = Mathf.Atan2((playerLoc.y - transform.position.y), (playerLoc.x - transform.position.x));
            Debug.Log(playerLoc.y - transform.position.y);
            Debug.Log(angleTar);
            Vector3 target = new Vector3(Mathf.Cos(angleTar - (Mathf.PI/2) + (Mathf.PI * j / numOfCircles)),  Mathf.Sin(angleTar - (Mathf.PI/2) + (Mathf.PI * j / numOfCircles)));
            for (int i = 0; i < number; i++)
            {
                float angle = (-i / (float)number) * 2 * Mathf.PI + Mathf.PI / 2;
                float xPos = centerPoint.x + radius * Mathf.Cos(angle);
                float yPos = centerPoint.y + radius * Mathf.Sin(angle);
                GameObject knife = knifePooler.GetDanmaku(1);
                if (knife != null)
                {
                    knife.AddComponent<BloodBullet>();
                    knife.GetComponent<Knife>().enabled = false;
                    BloodBullet bloodBulletScript = knife.GetComponent<BloodBullet>();
                    knife.transform.position = new Vector3(xPos, yPos, 0f);
                    bloodBulletScript.setTarget(target);
                    bloodBulletScript.setSpeed(j + 1);
                    knife.SetActive(true);
                }
                //yield return new WaitForSeconds(2f / number);
            }
            yield return new WaitForSeconds(1);
        }
        //yield return new WaitForSeconds(1);
    }


}
