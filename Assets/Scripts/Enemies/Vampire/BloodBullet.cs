using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBullet : MonoBehaviour, IEventListener, IPoolable {

    private const float baseSpeed = 10f;
    private float speed;
    private Transform target;
    private float timeCounter;
    private float width;
    private float height;
    private float moveSpeed;
    [SerializeField] private Vector3 centerPoint;
    [SerializeField] private Vector3 tar;
    private float timer;
    private int attack;
    private int ATK2Speed;

    private readonly int ATK_1 = 1;
    private readonly int ATK_2 = 2;

    private Animator anim;
    private bool deactivating;

    // The user of this attack (ie the vampire)
    private GameObject owner;
    
    void reset()
    {
        speed = baseSpeed;
    }

    void Awake() {
        timeCounter = 0;
        speed = 10;
        width = 1;
        height = 1;
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        EventMessanger.GetInstance().SubscribeEvent(typeof(DeleteAttacksEvent), this);
        centerPoint = transform.position;
        timeCounter = 0;
        speed = 10;
        deactivating = false;
        StopAllCoroutines();
        anim.SetTrigger("Appear");
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(DeleteAttacksEvent), this);
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void Update()
    {
        if (timer <= Time.time && !deactivating)
        {
            if (attack == ATK_1)
            {
                Attack1();
            }
            else if (attack == ATK_2)
            {
                Attack2();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (other.gameObject.layer != 8) {
                PlayerStatus playerStatus = other.gameObject.GetComponent<PlayerStatus>();
                if (playerStatus != null) {
                    playerStatus.TakeHit();
                } else {
                    Debug.LogError("No player status script found.");
                }
                speed = 0f;
                StartCoroutine(Deactivate());
            }
        }
    }

    IEnumerator Deactivate() {
        deactivating = true;
        anim.SetTrigger("Disappear");
        while(!(anim.GetCurrentAnimatorStateInfo(0).IsName("AwaitingDeactivation"))) {
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void setTarget(Vector3 target)
    {
        tar = Vector3.Normalize(target);

    }


    public void setSpeed(int s)
    {
        moveSpeed = s;
    }

    public void setTimer(float t)
    {
        timer = Time.time + t;
    }

    private void Attack1()
    {
        timeCounter += Time.deltaTime * speed;
        centerPoint += tar * moveSpeed * Time.deltaTime;
        float x = Mathf.Cos(timeCounter) * width;
        float y = Mathf.Sin(timeCounter) * height;
        transform.position = new Vector3(x + centerPoint.x, y + centerPoint.y, 0f);
    }

    private void Attack2()
    {
        transform.position = transform.position + tar * Time.deltaTime * ATK2Speed;
    }

    public void calcTarget(Vector3 toTarget)
    {
        tar = Vector3.Normalize(new Vector3(toTarget.x - transform.position.x, toTarget.y - transform.position.y));
    }

    public void setAttackOne()
    {
        attack = ATK_1;
    }

    public void setAttackTwo()
    {
        attack = ATK_2;
    }

    public void setATK2Speed(int theSpeed)
    {
        ATK2Speed = theSpeed;
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(DeleteAttacksEvent)) {
            DeleteAttacksEvent deleteAttacksEvent = e as DeleteAttacksEvent;
            if (deleteAttacksEvent.owner == this.owner) {
                StartCoroutine(Deactivate());
            }
        }
    }

}
