using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spore : MonoBehaviour
{
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
    public float timer;
    private int attack;
    private Renderer rend;

    private readonly int ATK_1 = 1;
    private readonly int ATK_2 = 2;

    void reset()
    {
        speed = baseSpeed;
    }

    void Start()
    {
        timeCounter = 0;
        speed = 10;
        width = 1;
        height = 1;
        centerPoint = transform.position;
        timer = 10.0f;
        rend = this.GetComponent<Renderer>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0.0f)
        {
            if (timer > -3)
            {
                // todo: have it fade a little each update
                Color col = rend.material.color;
                col.a -= 0.01f;
                rend.material.color = col;
            }
            else
            {
                // once completely transparent, delete this object
                gameObject.SetActive(false);
            }
        }
    }

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
                else
                {
                    Debug.LogError("No player status script found.");
                }
                speed = 0f;
                gameObject.SetActive(false);
            }
        }
    }

    public void setTimer(int t)
    {
        timer = Time.time + t;
    }
}
