using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBullet : MonoBehaviour {


    private const float baseSpeed = 10f;
    private float speed;
    private float angle;
    private float spinAnimationTimer = 0f;
    private float aimingAnimationTimer = 0f;
    private Quaternion aimedDirection;
    private Transform target;
    private float timeCounter;
    private float width;
    private float height;
    private float moveSpeed;
    private Vector3 centerPoint;
    private Vector3 tar;
    private float timer;
    private int attack;

    private readonly int ATK_1 = 1;
    private readonly int ATK_2 = 2;
    
    void reset()
    {
        speed = baseSpeed;
    }
    // Use this for initialization
    void Start () {
        timeCounter = 0;
        speed = 10;
        width = 1;
        height = 1;
        centerPoint = transform.position;
        timer = Time.time + 2f;
    }
	
	// Update is called once per frame
	void Update () {
        if (timer <= Time.time)
        {
            if (attack == ATK_1)
            {
                Attack1();
            }
            else if(attack == ATK_2)
            {
                Attack2();
                //Debug.Log("Attack 2");
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

    public void setTimer(int t)
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
        transform.position = transform.position + tar;
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
}
