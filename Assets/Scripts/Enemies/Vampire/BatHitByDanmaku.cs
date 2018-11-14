using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatHitByDanmaku : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("danmaku"))
        {
            col.gameObject.SetActive(false);
            gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
