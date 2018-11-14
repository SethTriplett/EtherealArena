using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonoBehaviour
{
    
    private ObjectPooler boulderPooler;
    private ObjectPooler sporePooler;
    public float timer;
    public float attackTimer;
    [SerializeField] float speed;
    bool attacked;
    public Vector3 target;
    private Vector3 moveVec;
    private bool right;
    private Animator animator;
    private Vector3 IDLE_TARGET_LOC;
    private const float IDLE_TIME = 3.0f;
    private const float DROP_ROCK_INTERVAL = 0.5f;
    private const float EXCRETE_SPORE = 0.3f;
    private Vector3 TOP_SCREEN_LOC;
    public int direction;
    private Vector3[] corners;
    private const int TOP_LEFT_IND = 0;
    private const int TOP_RIGHT_IND = 1;
    private const int BOTTOM_LEFT_IND = 3;
    private const int BOTTOM_RIGHT_IND = 2;
    public int currCornerInd = 0;
    private int cornersLen = 0;
    private LastAttack lastAtk;

    [SerializeField] private GameObject boulderPrefab;
    [SerializeField] private GameObject sporePrefab;
    [SerializeField] private Transform playerTransform;
    private int boulderIndex;
    private int sporeIndex;

    public enum BehaviorState
    {
        Idle,
        GoToIdlePos,
        DropRocksAttack,
        SporeAttack
    };

    private enum LastAttack
    {
        None,
        DropsRocksAttack,
        SporeAttack
    };

    public BehaviorState behaviorState;

    void Start()
    {
        IDLE_TARGET_LOC = new Vector3(-7.0f, 0.0f);
        TOP_SCREEN_LOC = new Vector3(-7.0f, 3.5f);
        boulderPooler = ObjectPooler.instance;
        sporePooler = ObjectPooler.instance;
        timer = IDLE_TIME;
        attacked = true;
        right = true;
        animator = GetComponent<Animator>();
        boulderIndex = boulderPooler.GetIndex(boulderPrefab);
        sporeIndex = sporePooler.GetIndex(sporePrefab);
        if (boulderIndex == -1)
        {
            Debug.LogError("Boulder not found in object pooler");
            boulderIndex = 0;
        }
        if (sporeIndex == -1)
        {
            Debug.LogError("Spore not found in object pooler");
            sporeIndex = 0;
        }
        behaviorState = BehaviorState.Idle;
        direction = -1;
        attackTimer = DROP_ROCK_INTERVAL;
        corners = new Vector3[4];
        corners[TOP_LEFT_IND] = new Vector3(-8.0f, 3.0f);
        corners[TOP_RIGHT_IND] = new Vector3(8.0f, 3.0f);
        corners[BOTTOM_RIGHT_IND] = new Vector3(8.0f, -3.0f);
        corners[BOTTOM_LEFT_IND] = new Vector3(-8.0f, -3.0f);
        currCornerInd = 0;
        cornersLen = corners.Length;
        lastAtk = LastAttack.None;
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
                    if (lastAtk == LastAttack.None || lastAtk == LastAttack.SporeAttack)
                    {
                        // todo: calculate timer based on hp
                        timer = 10.0f;
                        target = TOP_SCREEN_LOC;
                        behaviorState = BehaviorState.DropRocksAttack;
                    }
                    else
                    {
                        timer = 10.0f;
                        currCornerInd = TOP_LEFT_IND;
                        target = corners[currCornerInd];
                        behaviorState = BehaviorState.SporeAttack;
                    }
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
                lastAtk = LastAttack.DropsRocksAttack;
                attackTimer -= Time.deltaTime;
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
                        target = new Vector3(9 * direction, 3.5f);
                        if (transform.position.x <= -8.0f)
                        {
                            direction = 1;
                        }
                        if (transform.position.x >= 8.0f)
                        {
                            direction = -1;
                        }
                    }
                    if (attackTimer <= 0.0f)
                    {
                        GameObject aBoulder = boulderPooler.GetDanmaku(boulderIndex);
                        aBoulder.SetActive(true);
                        if (aBoulder != null)
                        {
                            Boulder boulderScript = aBoulder.GetComponent<Boulder>();
                            aBoulder.transform.position = gameObject.transform.position;

                        }
                        // reset the timer
                        attackTimer = DROP_ROCK_INTERVAL;
                    }

                }
                break;
            case BehaviorState.SporeAttack:
                lastAtk = LastAttack.SporeAttack;
                attackTimer -= Time.deltaTime;

                if (!closeEnoughToTarget())
                {
                    // move to corner of screen
                    move();
                }
                else
                {
                    // switch to next target
                    if (currCornerInd < cornersLen - 1)
                    {
                        currCornerInd += 1;
                        target = corners[currCornerInd];
                    }
                    else
                    {
                        target = IDLE_TARGET_LOC;
                        behaviorState = BehaviorState.GoToIdlePos;
                    }
                }
                if (attackTimer <= 0.0f)
                {
                    // let a spore loose
                    GameObject aSpore = boulderPooler.GetDanmaku(sporeIndex);
                    aSpore.SetActive(true);
                    if (aSpore != null)
                    {
                        Spore boulderScript = aSpore.GetComponent<Spore>();
                        aSpore.transform.position = gameObject.transform.position;

                    }
                    // reset the timer
                    attackTimer = EXCRETE_SPORE;
                }
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

    public void SetPlayerTransform(Transform player)
    {
        this.playerTransform = player;
    }

    private void faceLeft()
    {
        GetComponent<SpriteRenderer>().flipX = true;
        right = false;
    }

    private void faceRight()
    {
        GetComponent<SpriteRenderer>().flipX = false;
        right = true;
    }
}
