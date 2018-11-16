using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBulletController : MonoBehaviour, IPoolable, IEventListener {

    private Transform target;
    private float timeCounter;
    private float width;
    private float height;
    private float moveSpeed;
    private Vector3 tar;
    private int bounces;

    private GameObject owner;
    
    // Use this for initialization
    void Start () {
        moveSpeed = 5;
    }

    void OnEnable()
    {
        timeCounter = 0;
        if(bounces == 0)
        {
            bounces = 2;
        }
        moveSpeed = 5;

        EventMessanger.GetInstance().SubscribeEvent(typeof(DeleteAttacksEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(DeleteAttacksEvent), this);
    }
	
	// Update is called once per frame
	void Update () {
        if((transform.position.x > 7f || transform.position.x < -7f) && bounces > 0)
        {
            tar = new Vector3(-tar.x, tar.y);
            bounces--;
        }
        else if (bounces > 0 && (transform.position.y > 5.2f || transform.position.y < -5.2f))
        {
            tar = new Vector3(tar.x, -tar.y);
            bounces--;
        }
        Attack();
	}

    private void Attack()
    {
        transform.position = transform.position + tar * Time.deltaTime * moveSpeed;
    }

    public void setTarget(Vector3 target)
    {
        tar = Vector3.Normalize(target);

    }

    //gotta deal with player collision and I might possibly try and use this for bouncing the bat back into the arena
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.layer != 8)
            {
                PlayerStatus playerStatus = other.gameObject.GetComponent<PlayerStatus>();
                if (playerStatus != null)
                {
                    playerStatus.TakeHit();
                }
                else {
                    Debug.LogError("No player status script found.");
                }
                moveSpeed = 0f;
                gameObject.SetActive(false);
            }
        } 
    }

    public void setBounces(int numOfBounces)
    {
        bounces = numOfBounces;
    }

    public void setPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    private void Deactivate() {
        gameObject.SetActive(false);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(DeleteAttacksEvent)) {
            DeleteAttacksEvent deleteAttacksEvent = e as DeleteAttacksEvent;
            if (deleteAttacksEvent.owner == this.owner) {
                Deactivate();
            }
        }
    }
}
