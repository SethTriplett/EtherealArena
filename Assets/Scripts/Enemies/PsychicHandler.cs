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
	void Start () {
		phase = -1;
		bikeIndex = ObjectPooler.instance.GetIndex(bikeFab);
		tvIndex = ObjectPooler.instance.GetIndex(tvFab);
		knifeIndex = ObjectPooler.instance.GetIndex(knifeFab);
		
		Debug.Log (bikeIndex);

		StartCoroutine(toss());

	}
	
	// Update is called once per frame
	void Update () {


	}

	IEnumerator toss(){
		for (int i = 0; i < 15; i ++){
			createObject();
			yield return new WaitForSeconds(.05f);
		}	
	} 

	void createObject(){
		GameObject psychoball;
		if (Random.Range(0f,1f)>.5f){
			psychoball = ObjectPooler.instance.GetDanmaku(tvIndex);
		} else {
			psychoball = ObjectPooler.instance.GetDanmaku(bikeIndex);
		}
		
		psychoball.transform.position = transform.position + new Vector3 (0f,3f,0f);
		psychoball.GetComponent<PsychicTossObject>().setWangle(new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),0).normalized);
		psychoball.GetComponent<PsychicTossObject>().setSpeed(15f);
		psychoball.GetComponent<PsychicTossObject>().setOwner(gameObject);
		psychoball.SetActive(true);

	}
}
