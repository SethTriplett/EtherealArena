using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LarvaController : MonoBehaviour
{
<<<<<<< HEAD
    
    private ObjectPooler boulderPooler;
    private ObjectPooler sporePooler;
    public float timer;
    public float attackTimer;
=======

    private ObjectPooler bloodPooler;
    public float timer;
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
    [SerializeField] float speed;
    bool attacked;
    public Vector3 target;
    private Vector3 moveVec;
    private bool right;
    private Animator animator;
    private Vector3 IDLE_TARGET_LOC;
    private const float IDLE_TIME = 3.0f;
<<<<<<< HEAD
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
=======
    private Vector3 TOP_SCREEN_LOC;
    public int direction;

    [SerializeField] private GameObject bloodBulletPrefab;
    private int bloodBulletIndex;
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0

    public enum BehaviorState
    {
        Idle,
        GoToIdlePos,
        DropRocksAttack,
        SporeAttack
    };

<<<<<<< HEAD
    private enum LastAttack
    {
        None,
        DropsRocksAttack,
        SporeAttack
    };

=======
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
    public BehaviorState behaviorState;

    void Start()
    {
        IDLE_TARGET_LOC = new Vector3(-7.0f, 0.0f);
        TOP_SCREEN_LOC = new Vector3(-7.0f, 3.5f);
<<<<<<< HEAD
        boulderPooler = ObjectPooler.sharedPooler;
        sporePooler = ObjectPooler.sharedPooler;
=======
        bloodPooler = ObjectPooler.instance;
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
        timer = IDLE_TIME;
        attacked = true;
        right = true;
        animator = GetComponent<Animator>();
<<<<<<< HEAD
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
=======
        bloodBulletIndex = bloodPooler.GetIndex(bloodBulletPrefab);
        if (bloodBulletIndex == -1)
        {
            Debug.LogError("BloodBullet not found in object pooler");
            bloodBulletIndex = 0;
        }
        behaviorState = BehaviorState.Idle;
        direction = -1;
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
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
<<<<<<< HEAD
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
=======
                    // todo: calculate timer based on hp
                    timer = 10.0f;
                    target = TOP_SCREEN_LOC;
                    behaviorState = BehaviorState.DropRocksAttack;
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
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
<<<<<<< HEAD
                lastAtk = LastAttack.DropsRocksAttack;
                attackTimer -= Time.deltaTime;
=======
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
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
<<<<<<< HEAD

                        // oscillate between left and right of screen
                        // todo: have it check if collided instead
                        target = new Vector3(9 * direction, 3.5f);
=======
                        // oscillate between left and right of screen
                        // todo: have it check if collided instead
                        target = new Vector3(9*direction, 3.5f);
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
                        if (transform.position.x <= -8.0f)
                        {
                            direction = 1;
                        }
<<<<<<< HEAD
                        if (transform.position.x >= 8.0f)
=======
                        if(transform.position.x >= 8.0f)
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
                        {
                            direction = -1;
                        }
                    }
<<<<<<< HEAD
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
=======
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0

                }
                break;
            case BehaviorState.SporeAttack:
<<<<<<< HEAD
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
=======
                // todo: change logic. this is a placeholder
                behaviorState = BehaviorState.Idle;
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
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

<<<<<<< HEAD
    public void SetPlayerTransform(Transform player)
    {
        this.playerTransform = player;
    }

=======
>>>>>>> dfe80e176381487f48034affdf40e57dfbabf4f0
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
