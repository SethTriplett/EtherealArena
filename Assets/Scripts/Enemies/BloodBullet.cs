using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBullet : MonoBehaviour {


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
    
    void reset()
    {
        speed = baseSpeed;
    }

    void Start () {
        timeCounter = 0;
        speed = 10;
        //ATK2Speed = 3;
        width = 1;
        height = 1;
        //centerPoint = transform.position;
    }

    void OnEnable()
    {
        centerPoint = transform.position;
        //timer = Time.time + 2f;
        timeCounter = 0;
        speed = 10;
        //Debug.Log(attack);
    }

    void Update()
    {
        if (timer <= Time.time)
        {
            if (attack == ATK_1)
            {
                Attack1();
            }
            else if (attack == ATK_2)
            {
                Attack2();
                //Debug.Log("Attack 2");
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
                gameObject.SetActive(false);
            }
        }
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
        //Debug.Log("set attack 1");
    }

    public void setAttackTwo()
    {
        attack = ATK_2;
    }

    public void setATK2Speed(int theSpeed)
    {
        ATK2Speed = theSpeed;
    }
}
