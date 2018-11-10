using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicHandler : MonoBehaviour {

	int phase;
	
	int bikeIndex;
	int tvIndex;
	int knifeIndex;

[SerializeField] GameObject bikeFab;

[SerializeField] GameObject tvFab;
	
[SerializeField] GameObject knifeFab;

    [SerializeField] private Transform playerTransform;

	void Start () {
		phase = -1;
		bikeIndex = ObjectPooler.instance.GetIndex(bikeFab);
		tvIndex = ObjectPooler.instance.GetIndex(tvFab);
		knifeIndex = ObjectPooler.instance.GetIndex(knifeFab);

		StartCoroutine(JunkStream());
        StartCoroutine(KnifesEdge());
	}
	
	// Update is called once per frame
	void Update () {


	}

    public void SetPlayerTransform(Transform playerTransform) {
        this.playerTransform = playerTransform;
    }

	IEnumerator toss(){
        do {
			createTossObject();
			yield return new WaitForSeconds(2.5f);
		} while (true);
	} 

	void createTossObject(){
		GameObject psychoball;
		if (Random.Range(0f,1f)>.5f){
			psychoball = ObjectPooler.instance.GetDanmaku(tvIndex);
		} else {
			psychoball = ObjectPooler.instance.GetDanmaku(bikeIndex);
		}
        
        if (psychoball == null) {
            Debug.LogError("Telekinetic object not pulled from pool.");
            return;
        }
        PsychicTossObject psychicObjectScript = psychoball.GetComponent<PsychicTossObject>();
        if (psychicObjectScript == null) {
            Debug.LogError("Psychic object script not found.");
            return;
        }
		psychicObjectScript.setOwner(gameObject);
        Vector3 randAngle = new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0).normalized;
		psychicObjectScript.setWangle(randAngle);
        Vector3 randCenterPoint = new Vector3(Random.Range(-1.5f,1.5f),Random.Range(-1f,1f),0);
		psychoball.transform.position = randCenterPoint - (10 * randAngle);
		psychicObjectScript.setSpeed(7f);
		psychoball.SetActive(true);
	}

	void createStreamObject(){
		GameObject psychoball;
		if (Random.Range(0f,1f)>.5f){
			psychoball = ObjectPooler.instance.GetDanmaku(tvIndex);
		} else {
			psychoball = ObjectPooler.instance.GetDanmaku(bikeIndex);
		}
		
        if (psychoball == null) {
            Debug.LogError("Telekinetic object not pulled from pool.");
            return;
        }
        PsychicTossObject psychicObjectScript = psychoball.GetComponent<PsychicTossObject>();
        if (psychicObjectScript == null) {
            Debug.LogError("Psychic object script not found.");
            return;
        }
		psychicObjectScript.setOwner(gameObject);
        Vector3 randAngle = new Vector3(1f,Random.Range(-.05f,.05f),0).normalized;
		psychicObjectScript.setWangle(randAngle);
		psychoball.transform.position = new Vector3(-8.5f, Random.Range(-5f, -0.5f), 0f);
		psychicObjectScript.setSpeed(8f);
		psychoball.SetActive(true);
	}

    private IEnumerator JunkStream() {
        do {
            createStreamObject();
            yield return new WaitForSeconds(0.02f);
        } while (true);
    }

    private IEnumerator KnifesEdge() {
        do {
            StartCoroutine(KnifeSet(1));
            yield return new WaitForSeconds(1.5f);
        } while (true);
    }

    IEnumerator KnifeSet(int number) {
        Vector3 targetVector = playerTransform.position - transform.position;
        targetVector.Normalize();
        Vector3 perpendicularVector = new Vector3(-targetVector.y, targetVector.x, 0f);
        Vector3[] knifePos = new Vector3[number];
        GameObject[] knives = new GameObject[number];
        // Create a number of knives that fan out.
        for (int i = 0; i < number; i++) {
            float adjustedIndex = i - ((number - 1) / 2f);
            knifePos[i] = adjustedIndex * perpendicularVector + targetVector + transform.position;
            // Add some random positioning
            knifePos[i] += new Vector3(Random.Range(-.5f, 0f), Random.Range(-1.5f, 1.5f), 0f);
            GameObject knife = ObjectPooler.instance.GetDanmaku(knifeIndex);
            if (knife != null) {
                Knife knifeScript = knife.GetComponent<Knife>();
                if (knifeScript == null) {
                    Debug.LogError("Knife script not found.");
                } else {
                    knife.transform.position = gameObject.transform.position;
                    // Aim directly left
                    knife.transform.rotation = Quaternion.Euler(0, 0, 180);
                    knifeScript.SetTracking(false);
                    knifeScript.SetAimedAngle(180f);
                    knifeScript.StartAimingAnimation(1.5f);
                    knives[i] = knife;
                    knife.SetActive(true);
                }
            } else {
                Debug.LogError("Knife missing from object pooler.");
            }
        }
        // Lerp to be in front of the user
        for (int j = 0; j < 30; j++) {
            for (int i = 0; i < number; i++) {
                GameObject knife = knives[i];
                knife.transform.position = Vector3.LerpUnclamped(knife.transform.position, knifePos[i], 0.1f);
            }
            yield return new WaitForSeconds(0.016f);
        }
        for (int i = 0; i < number; i++) {
            knives[i].transform.position = knifePos[i];
        }
    }

}
