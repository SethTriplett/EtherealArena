using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicHandler : MonoBehaviour, IEventListener {

	int bikeIndex;
	int tvIndex;
	int knifeIndex;

    [SerializeField] GameObject bikeFab;
    [SerializeField] GameObject tvFab;
    [SerializeField] GameObject knifeFab;

	void Awake () {
		bikeIndex = ObjectPooler.instance.GetIndex(bikeFab);
		tvIndex = ObjectPooler.instance.GetIndex(tvFab);
		knifeIndex = ObjectPooler.instance.GetIndex(knifeFab);
	}

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PhaseTransitionEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerDefeatEvent), this);
    }
    
    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PhaseTransitionEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerDefeatEvent), this);

        EventMessanger.GetInstance().TriggerEvent(new DeleteAttacksEvent(gameObject));
    }

    private void TransitionPhase(int nextPhase) {
        StopAllCoroutines();
        EventMessanger.GetInstance().TriggerEvent(new DeleteAttacksEvent(gameObject));
        AudioManager.GetInstance().StopSound(Sound.WhooshLarge);
        switch(nextPhase) {
            case 1:
                StartPhase1();
                break;
            case 2:
                StartPhase2();
                break;
            case 3:
                StartPhase3();
                break;
        }
    }
	
    public void StartPhase1() {
        StartCoroutine(toss());
    }

    public void StartPhase2() {
        StartCoroutine(IronCurtain());
        StartCoroutine(PsychicFling());
    }

    public void StartPhase3() {
        StartCoroutine(KnifesEdge());
        StartCoroutine(JunkStream());
    }

	IEnumerator toss(){
        do {
			createTossObject();
			yield return new WaitForSeconds(1f);
            AudioManager.GetInstance().PlaySound(Sound.WhooshLarge);
			yield return new WaitForSeconds(1.5f);
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
            print (psychoball);
            print (tvIndex);
            print (bikeIndex);
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

    // Knife attack in second phase
    private IEnumerator IronCurtain() {
        do {
            float angle;
            Vector3 centerPoint;
            Vector3 startPoint;
            if (Random.Range(0f, 1f) > 0.5f) {
                angle = 270f;
                centerPoint = new Vector3(0f, 5f, 0f);
                startPoint = new Vector3(0f, 6f, 0f);
            } else {
                angle = 90f;
                centerPoint = new Vector3(0f, -5f, 0f);
                startPoint = new Vector3(0f, -6f, 0f);
            }
            StartCoroutine(KnifeSet(24, angle, centerPoint, startPoint, 2.5f, true));
            yield return new WaitForSeconds(3f);
        } while (true);
    }

    // Horizontal variant used in second phase
    private IEnumerator PsychicFling() {
        do {
            createFlingObject();
            yield return new WaitForSeconds(1f);
            AudioManager.GetInstance().PlaySound(Sound.WhooshLarge);
            yield return new WaitForSeconds(2f);
        } while (true);
    }
	
    void createFlingObject(){
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
        Vector3 randAngle = new Vector3(Random.Range(-1f, 1f), Random.Range(-0.2f, 0.2f), 0).normalized;
		psychicObjectScript.setWangle(randAngle);
        Vector3 randCenterPoint = new Vector3(Random.Range(-1.5f,1.5f),Random.Range(-1f,1f),0);
		psychoball.transform.position = randCenterPoint - (10 * randAngle);
		psychicObjectScript.setSpeed(6f);
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
            AudioManager.GetInstance().PlaySound(Sound.WhooshLarge);
        } while (true);
    }

    private IEnumerator KnifesEdge() {
        do {
            Vector3 randomHeight;
            if (Random.Range(0f, 1f) > 0.5f) {
                randomHeight = new Vector3(transform.position.x - 1f, transform.position.y + 1f, 0f);
            } else {
                randomHeight = new Vector3(transform.position.x - 1f, transform.position.y - 0.5f, 0f);
            }
            StartCoroutine(KnifeSet(1, 180f, randomHeight, transform.position, 1.5f, false));
            yield return new WaitForSeconds(1.5f);
        } while (true);
    }

// I'm starting to overload this method
// Edit: I'm totally overloading this method
    IEnumerator KnifeSet(int number, float angle, Vector3 centerPos, Vector3 startPos, float delay, bool includeGap) {
        Vector3 perpendicularVector = new Vector3(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0f).normalized;
        Vector3[] knifePos = new Vector3[number];
        GameObject[] knives = new GameObject[number];
        // Create a number of knives that fan out.
        for (int i = 0; i < number; i++) {
            float adjustedIndex = i - ((number - 1) / 2f);
            knifePos[i] = adjustedIndex * perpendicularVector * 0.6f + centerPos;
            GameObject knife = ObjectPooler.instance.GetDanmaku(knifeIndex);
            if (knife != null) {
                Knife knifeScript = knife.GetComponent<Knife>();
                if (knifeScript == null) {
                    Debug.LogError("Knife script not found.");
                } else {
                    knife.transform.position = startPos;
                    // Aim directly left
                    knife.transform.rotation = Quaternion.Euler(0, 0, angle);
                    knifeScript.SetOwner(gameObject);
                    AudioManager.GetInstance().PlaySound(Sound.KnifeDraw);
                    knifeScript.SetTracking(false);
                    knifeScript.SetAimedAngle(angle);
                    knifeScript.StartAimingAnimation(delay);
                    knives[i] = knife;
                    knife.SetActive(true);
                }
            } else {
                Debug.LogError("Knife missing from object pooler.");
            }
        }
        // This is a fragile work around. Relocate it later
        if (includeGap) {
            // Turn off a random group of knives
            int randIndex = Random.Range(1, 4);
            for (int x = randIndex * 4; x < (randIndex + 1) * 4; x++) {
                knives[x].SetActive(false);
            }
        }
        // Lerp to be in front of the user
        for (int j = 0; j < 30; j++) {
            for (int i = 0; i < number; i++) {
                if (knives[i].activeInHierarchy) {
                    GameObject knife = knives[i];
                    knife.transform.position = Vector3.LerpUnclamped(knife.transform.position, knifePos[i], 0.1f);
                }
            }
            yield return new WaitForSeconds(0.016f);
        }
        for (int i = 0; i < number; i++) {
            if (knives[i].activeInHierarchy) {
                knives[i].transform.position = knifePos[i];
            }
        }
    }

    private void KO() {
        StopAllCoroutines();
        EventMessanger.GetInstance().TriggerEvent(new DeleteAttacksEvent(gameObject));
    }

    private void FightOver() {
        StopAllCoroutines();
    }

    private void StartDelayed() {
        float duration = 2.75f;
        StartCoroutine(StartDelayedSubroutine(duration));
    }

    private IEnumerator StartDelayedSubroutine(float duration) {
        while (duration > 0) {
            duration -= Time.deltaTime;
            yield return null;
        }
        TransitionPhase(1);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PhaseTransitionEvent)) {
            PhaseTransitionEvent phaseTransitionEvent = e as PhaseTransitionEvent;
            TransitionPhase(phaseTransitionEvent.nextPhase);
        } else if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            StartDelayed();
            AudioManager.GetInstance().StartMusic(Soundtrack.PsychicTheme);
            AudioManager.GetInstance().StopMusic(Soundtrack.TitleTheme);
        } else if (e.GetType() == typeof(PlayerVictoryEvent)) {
            KO();
        } else if (e.GetType() == typeof(PlayerDefeatEvent)) {
            FightOver();
        }
    }

}
