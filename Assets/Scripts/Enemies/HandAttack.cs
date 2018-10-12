using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAttack : MonoBehaviour {

    private float timer;
    private GameObject player;
	void Start () {
        timer = 2f;
	}
	
	void Update () {
		if(player != null)
        {
            player.transform.position = transform.position;
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col);
        if(col.gameObject.name.Equals("Player"))
        {
            player = col.gameObject;
            transform.parent.gameObject.GetComponent<FirstBossController>().playerGrabbed();
            Debug.Log("Should attack");
        }
    }

    public void release()
    {
        if (player != null)
        {
            player.GetComponent<PlayerStatus>().TakeHit();
            transform.parent.gameObject.GetComponent<EnemyStatus>().HealHealthPortion(0.1f);
        }
        player = null;
    }

}
