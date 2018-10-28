using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonoBehaviour
{

    private ObjectPooler bloodPooler;
    public float timer;
    [SerializeField] float speed;
    bool attacked;
    public Vector3 target;
    private Vector3 moveVec;
    private bool right;
    private Animator animator;
    private Vector3 IDLE_TARGET_LOC;
    private const float IDLE_TIME = 3.0f;
    private Vector3 TOP_SCREEN_LOC;
    public int direction;

    [SerializeField] private GameObject bloodBulletPrefab;
    private int bloodBulletIndex;

    public enum BehaviorState
    {
        Idle,
        GoToIdlePos,
        DropRocksAttack,
        SporeAttack
    };

    public BehaviorState behaviorState;

    void Start()
    {
        IDLE_TARGET_LOC = new Vector3(-7.0f, 0.0f);
        TOP_SCREEN_LOC = new Vector3(-7.0f, 3.5f);
        bloodPooler = ObjectPooler.sharedPooler;
        timer = IDLE_TIME;
        attacked = true;
        right = true;
        animator = GetComponent<Animator>();
        bloodBulletIndex = bloodPooler.GetIndex(bloodBulletPrefab);
        if (bloodBulletIndex == -1)
        {
            Debug.LogError("BloodBullet not found in object pooler");
            bloodBulletIndex = 0;
        }
        behaviorState = BehaviorState.Idle;
        direction = -1;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        switch (behaviorState)
        {
            case BehaviorState.Idle:
                if (timer <= 0.0f)
                {
                    // todo: calculate timer based on hp
                    timer = 10.0f;
                    target = TOP_SCREEN_LOC;
                    behaviorState = BehaviorState.DropRocksAttack;
                }
                break;
            case BehaviorState.GoToIdlePos:
                if (closeEnoughToTarget())
                {
                    timer = IDLE_TIME;
                    behaviorState = BehaviorState.Idle;
                }
                else
                {
                    target = IDLE_TARGET_LOC;
                    move();
                }
                break;
            case BehaviorState.DropRocksAttack:
                if (timer <= 0.0f)
                {
                    target = IDLE_TARGET_LOC;
                    behaviorState = BehaviorState.GoToIdlePos;
                }
                else
                {
                    if (!closeEnoughToTarget())
                    {
                        // move to top of screen
                        move();
                    }
                    else
                    {
                        // oscillate between left and right of screen
                        // todo: have it check if collided instead
                        target = new Vector3(9*direction, 3.5f);
                        if (transform.position.x <= -8.0f)
                        {
                            direction = 1;
                        }
                        if(transform.position.x >= 8.0f)
                        {
                            direction = -1;
                        }
                    }

                }
                break;
            case BehaviorState.SporeAttack:
                // todo: change logic. this is a placeholder
                behaviorState = BehaviorState.Idle;
                break;
        }
    }

    private bool closeEnoughToTarget()
    {
        if (Vector3.Distance(target, transform.position) < 0.5f)
        {
            return true;
        }
        return false;
    }

    private void move()
    {
        moveVec = Vector3.Normalize(target - transform.position);
        transform.position += moveVec * Time.deltaTime * speed;
    }

    /*private void attack()
    {
        if (!secondForm)
        {
            StartCoroutine(CircleHell(transform.position, 4, .5f, 4, playerTransform.position));
        }
        else
        {
            StartCoroutine(CircleHell(transform.position, 5, .5f, 5, playerTransform.position));
        }
    }*/

    private void faceLeft()
    {
        GetComponent<SpriteRenderer>().flipX = true;
        //hand.transform.position = hand.transform.position + new Vector3(-2, 0, 0);
        right = false;
    }

    private void faceRight()
    {
        GetComponent<SpriteRenderer>().flipX = false;
        //hand.transform.position = hand.transform.position + new Vector3(2, 0, 0);
        right = true;
    }
}
