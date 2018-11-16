using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//A script to run the first boss, the Vampire: Count Versum the 5th
//I'm also putting his in here in order for a difference to be made and hopefully updated in git
public class VampireController : MonoBehaviour, IEventListener {

    private ObjectPooler bloodPooler;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool secondForm;
    float timer;
    string curMeth;
    [SerializeField] int GAS; //Grab Attack Speed
    [SerializeField] float speed;
    bool attacked;
    private Vector3 target;
    private Vector3 moveVec;
    [SerializeField] GameObject hand;
    private bool right;
    private bool resetMove;
    private bool grabbed;
    private Animator animator;
    List<Vector3> moveLoc;
    private int curPos; //Position in the list that the boss is currently at

    private bool canTurn;
    private float GAWait;
    private float GATime;
    private bool GADone;
    private bool blinged;

    private EnemyStatus status;

    [SerializeField] GameObject bling;

    [SerializeField] private GameObject bloodBulletPrefab;
    [SerializeField] private GameObject batBulletPrefab;
    private int bloodBulletIndex;
    private int batBulletIndex;

    private bool run;
    private bool bossPaused;


    void Start() {
        timer = 0.5f;
        bloodPooler = ObjectPooler.instance;
        curMeth = "attack";
        attacked = true;
        resetMove = false;
        right = true;
        GADone = false;
        grabbed = false;
        run = false;
        blinged = false;
        GAS = 15;
        animator = GetComponent<Animator>();

        bloodBulletIndex = bloodPooler.GetIndex(bloodBulletPrefab);
        if (bloodBulletIndex == -1) {
            Debug.LogError("Blood Bullet index not found.");
            bloodBulletIndex = 0;
        }
        batBulletIndex = bloodPooler.GetIndex(batBulletPrefab);
        if (batBulletIndex == -1) {
            Debug.LogError("Bat Bullet index not found.");
            batBulletIndex = 0;
        }

        status = GetComponent<EnemyStatus>();
        canTurn = true;
        if (moveLoc == null || moveLoc.Count != 4)
        {
            moveLoc = new List<Vector3>();
            moveLoc.Add(new Vector3(-5.2f, -3.2f));
            moveLoc.Add(new Vector3(5, -3));
            moveLoc.Add(new Vector3(3.3f, 3.3f));
            moveLoc.Add(new Vector3(-3, 3));
        }

    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PhaseTransitionEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerDefeatEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PhaseTransitionEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerDefeatEvent), this);

        EventMessanger.GetInstance().TriggerEvent(new DeleteAttacksEvent(gameObject));
    }

    // Update is called once per frame
    void Update() {
        if (!bossPaused) {
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            animator.SetTrigger("EnterIdle");
            setTimer();
        }
        else if (run)
        {
            if (curMeth.Equals("move"))
            {
                move();
            }
            else if (curMeth.Equals("attack") && !attacked)
            {
                attack();
                attacked = true;
            }
            else if (curMeth.Equals("AG") && !GADone)
            {
                animator.SetTrigger("EnterChargeUp");
                GrabAttack();
            }
            else if (curMeth.Equals("AG") && GADone)
            {
                animator.ResetTrigger("EnterChargeUp");
            }
            if (playerTransform.position.x < transform.position.x && right && canTurn)
            {
                faceLeft();
            }
            else if (playerTransform.position.x > transform.position.x && !right && canTurn)
            {
                faceRight();
            }
        }
        else
        {
            Wait();
        }
    }


    private void setTimer()
    {
        //hand.GetComponent<HandAttack>().release();
        canTurn = true;
        grabbed = false;
        run = true;
        blinged = false;
        if (!resetMove)
        {
            if (!attacked && !GADone)
            {
                timer = 10f;
                float helper = Random.Range(0, 3);
                if (helper == 0)
                {
                    curMeth = "AG";
                    timer = 10f;
                    canTurn = false;
                    GAWait = Time.time + 1f;
                    GATime = Time.time + 2.5f;
                    float angleTar = Mathf.Atan2((playerTransform.position.y - transform.position.y), (playerTransform.position.x - transform.position.x));
                    target = Vector3.Normalize(new Vector3(playerTransform.position.x - transform.position.x, playerTransform.position.y - transform.position.y));
                }
                else
                {
                    curMeth = "attack";
                }
            }
            else
            {
                timer = 7f;
                attacked = false;
                GADone = false;
                curMeth = "move";
                moveVec = chooseLocation();
            }
            hand.SetActive(false);
        } else {
            timer = 7f;
            attacked = false;
            GADone = false;
            resetMove = false;
            blinged = false;
            curMeth = "move";
            moveVec = chooseLocation();
        }
    }
    
    //A method to randomly choose the next location to move to and returns the vector point from the current position to the
    //new position
    private Vector3 chooseLocation()
    {
        int helper = Random.Range(0, moveLoc.Count);
        while (helper == curPos)
        {
            helper = Random.Range(0, moveLoc.Count);
        }
        Vector3 retVec = moveLoc[helper];
        target = retVec;
        curPos = helper;
        return Vector3.Normalize(new Vector3(retVec.x - transform.position.x, retVec.y - transform.position.y));
    }

    private void move()
    {
        moveVec = Vector3.Normalize(target - transform.position);
        if (!(Mathf.Abs(target.x - transform.position.x) < .25f) || !(Mathf.Abs(target.y - transform.position.y) < .25f))
        {
            transform.position += moveVec * Time.deltaTime * speed;
        }
        else
        {
            transform.position = target;
            timer = -1;
        }
    }

    private void attack()
    {
        if (status.getPhase() == 1)
        {
            if (Random.Range(0, 2) == 0)
            {
                StartCoroutine(CircleHell(transform.position, 5, .5f, 5, playerTransform.position));
            }
            else 
            {
                StartCoroutine(BloodBolts(8, playerTransform, transform.position, 2.1f, 1));
            }
        }
        else if (status.getPhase() == 2)
        {
            int helper = Random.Range(0, 4);
            if (helper == 0)
            {
                StartCoroutine(CircleHell(transform.position, 6, .6f, 6, playerTransform.position));
            }
            else if (helper < 3)
            {
                StartCoroutine(BloodBolts(12, playerTransform, transform.position, 2.1f, 2));
            }
            else
            {
                StartCoroutine(BatAttack(6, playerTransform, 3));
            }
        }
        else
        {
            StartCoroutine(BatAttack(10, playerTransform, 4));
        }
    }

    IEnumerator CircleHell(Vector3 centerPoint, int number, float radius, int numOfCircles, Vector3 playerLoc)
    {
        for (int j = 0; j < numOfCircles; j++)
        {
            float angleTar = Mathf.Atan2((playerLoc.y - transform.position.y), (playerLoc.x - transform.position.x));
            Vector3 bulletTarget = new Vector3(Mathf.Cos(angleTar - (Mathf.PI / 2) + (Mathf.PI * j / numOfCircles)), Mathf.Sin(angleTar - (Mathf.PI / 2) + (Mathf.PI * j / numOfCircles)));
            for (int i = 0; i < number; i++)
            {
                float angle = (-i / (float)number) * 2 * Mathf.PI + Mathf.PI / 2;
                float xPos = centerPoint.x + radius * Mathf.Cos(angle);
                float yPos = centerPoint.y + radius * Mathf.Sin(angle);
                GameObject bloodBullet = bloodPooler.GetDanmaku(bloodBulletIndex);
                if (bloodBullet != null)
                {
                    BloodBullet bloodBulletScript = bloodBullet.GetComponent<BloodBullet>();
                    bloodBullet.transform.position = new Vector3(xPos, yPos, 0f);
                    bloodBulletScript.SetOwner(gameObject);
                    bloodBulletScript.setTarget(bulletTarget);
                    bloodBulletScript.setAttackOne();
                    bloodBulletScript.setSpeed(j + 3);
                    bloodBulletScript.setTimer(2);
                    bloodBullet.SetActive(true);
                }
                //yield return new WaitForSeconds(2f / number);
            }
            AudioManager.GetInstance().PlaySound(Sound.VampireBullet);
            yield return new WaitForSeconds(0.5f);
            animator.SetTrigger("EnterChargeUp");
            yield return new WaitForSeconds(0.5f);
            if (j > 0) {
                animator.SetTrigger("EnterAttack");
            }
        }
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("EnterChargeUp");
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("EnterAttack");
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("EnterIdle");
    }
     //WIP for another attack
    IEnumerator BloodBolts(int number, Transform playerLoc, Vector3 centerPoint, float radius, int speedIncrease)
    {
        for (int i = 0; i < number; i++)
        {
            timer = timer + .1f;
            float angleTar = Mathf.Atan2((playerLoc.position.y - transform.position.y), (playerLoc.position.x - transform.position.x));
            Vector3 bulletTarget = radius * new Vector3(Mathf.Cos(angleTar - (Mathf.PI / 2) + (Mathf.PI * i)), Mathf.Sin(angleTar - (Mathf.PI / 2) + (Mathf.PI * i))) + transform.position;
            GameObject bloodBullet = bloodPooler.GetDanmaku(bloodBulletIndex);
            if (bloodBullet != null)
            {
                BloodBullet bloodBulletScript = bloodBullet.GetComponent<BloodBullet>();
                bloodBullet.transform.position = bulletTarget;
                bloodBulletScript.SetOwner(gameObject);
                bloodBulletScript.calcTarget(playerLoc.position);
                bloodBulletScript.setAttackTwo();
                bloodBulletScript.setATK2Speed(15 + speedIncrease);
                bloodBulletScript.setTimer(.5f);
                bloodBullet.SetActive(true);
            }
            AudioManager.GetInstance().PlaySound(Sound.VampireBullet);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator BatAttack(int numOfBats, Transform playerLoc, int numOfBounces)
    {
        AudioManager.GetInstance().PlaySound(Sound.VampireBats);
        float angleTar = Mathf.Atan2((playerLoc.position.y - transform.position.y), (playerLoc.position.x - transform.position.x));
        for (int i = 0; i < numOfBats; i++)
        {
            Vector3 batTarget = new Vector3(Mathf.Cos(angleTar - (Mathf.PI / 2) + (Mathf.PI * i / numOfBats)), Mathf.Sin(angleTar - (Mathf.PI / 2) + (Mathf.PI * i / numOfBats)));
            GameObject batBullet = bloodPooler.GetDanmaku(batBulletIndex);
            if(batBullet != null)
            {
                BatBulletController batBulCon = batBullet.GetComponent<BatBulletController>();
                batBulCon.SetOwner(gameObject);
                batBulCon.setTarget(batTarget);
                batBulCon.setPos(transform.position);
                batBulCon.setBounces(numOfBounces);
                batBullet.SetActive(true);
            }
            yield return new WaitForSeconds(.2f);
        }
        for (int i = numOfBats - 1; i > 0; i--)
        {
            Vector3 batTarget = new Vector3(Mathf.Cos(angleTar - (Mathf.PI / 2) + (Mathf.PI * i / numOfBats)), Mathf.Sin(angleTar - (Mathf.PI / 2) + (Mathf.PI * i / numOfBats)));
            GameObject batBullet = bloodPooler.GetDanmaku(batBulletIndex);
            if (batBullet != null)
            {
                BatBulletController batBulCon = batBullet.GetComponent<BatBulletController>();
                batBulCon.SetOwner(gameObject);
                batBulCon.setTarget(batTarget);
                batBulCon.setPos(transform.position);
                batBulCon.setBounces(numOfBounces);
                batBullet.SetActive(true);
            }
            yield return new WaitForSeconds(.2f);
        }
    }

    void GrabAttack()
    {
        if (GATime - Time.time <= 0)
        {
            attacked = true;
            hand.SetActive(false);
            target = Vector3.Normalize(moveLoc[curPos] - transform.position);
            transform.position = transform.position + target * GAS * Time.deltaTime;
            if(Mathf.Abs(moveLoc[curPos].y - transform.position.y) < .25f && Mathf.Abs(moveLoc[curPos].x - transform.position.x) < .25f)
            {
                transform.position = moveLoc[curPos];
                if (GADone == false)
                {
                    timer = .25f;
                }
                GADone = true;
            }
        } else if(!blinged)
        {
            blinged = true;
            bling.SetActive(true);
            //bling.GetComponent<Animator>().ResetTrigger("Glint");
            bling.GetComponent<Animator>().Play("Glint", -1, 0f);
        }
        if (GAWait - Time.time <= 0) {
	        animator.ResetTrigger("EnterChargeUp");
            animator.SetTrigger("EnterIdle");
            if (hand.activeSelf != true && !attacked) {
                hand.SetActive(true);
            }
            if (canTurn) {
                canTurn = false;
            }
            if (grabbed) {
                Vector3 toCenter = new Vector3(-transform.position.x, -transform.position.y);
                toCenter = Vector3.Normalize(toCenter);
                if (!(Mathf.Abs(transform.position.x) < .15f) || !(Mathf.Abs(transform.position.y) < .15f)) {
                    transform.position = transform.position + toCenter * Time.deltaTime * speed;
                }
                else if (timer - Time.time < 1.5f) {
                    hand.GetComponent<HandAttack>().release();
                    hand.SetActive(false);
                    GATime = 0;
                    //setTimer();
                }
            }
            else {
                transform.position = transform.position + target * GAS * Time.deltaTime;
            }
        } 
    }

    private void faceLeft()
    {
        GetComponent<SpriteRenderer>().flipX = true;
        hand.transform.position = hand.transform.position + new Vector3(-2, 0, 0);
        bling.transform.position = bling.transform.position + new Vector3(-1, 0, 0);
        right = false;
    }

    private void faceRight()
    {
        GetComponent<SpriteRenderer>().flipX = false;
        hand.transform.position = hand.transform.position + new Vector3(2, 0, 0);
        bling.transform.position = bling.transform.position + new Vector3(1, 0, 0);
        right = true;
    }

    public void playerGrabbed()
    {
        grabbed = true;
        if(GATime - Time.time < 1f || timer - Time.time < 2f)
        {
            GATime = Time.time + 1.5f;
            timer = Time.time + 4f;
        }
    }

    public void Wait()
    {

    }

    private void TransitionPhases(int nextPhase) {
        if (nextPhase > 1) {
            AudioManager.GetInstance().PlaySound(Sound.VampireHiss);
        }
        animator.ResetTrigger("EnterIdle");
        animator.ResetTrigger("EnterChargeUp");
        animator.SetTrigger("EnterAttack");
        StopFunction();
        StopAllCoroutines();
    }

    private void DelayStart() {
        float duration = 2.75f;
        StartCoroutine(DelayStartSubroutine(duration));
    }

    private IEnumerator DelayStartSubroutine(float duration) {
        bossPaused = true;
        while (duration > 0) {
            duration -= Time.deltaTime;
            yield return null;
        }
        bossPaused = false;
    }

    public void KO() {
        StopAllCoroutines();
        StopFunction();
        AudioManager.GetInstance().PlaySound(Sound.VampireHiss);
        animator.ResetTrigger("EnterIdle");
        animator.ResetTrigger("EnterChargeUp");
        animator.SetTrigger("EnterAttack");
        bossPaused = true;
    }
    
    public void FightOver() {
        StopFunction();
        bossPaused = true;
    }

    public void StopFunction()
    {
        hand.SetActive(false);
        run = false;
        timer = 1.5f;
        animator.SetTrigger("EnterIdle");
        StopAttacks();
        resetMove = true;
    }

    public void StopAttacks() {
        EventMessanger.GetInstance().TriggerEvent(new DeleteAttacksEvent(gameObject));
        AudioManager.GetInstance().StopSound(Sound.VampireBats);
    }

    public void SetPlayerTransform(Transform playerTransform) {
        this.playerTransform = playerTransform;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PhaseTransitionEvent)) {
            PhaseTransitionEvent phaseTransitionEvent = e as PhaseTransitionEvent;
            TransitionPhases(phaseTransitionEvent.nextPhase);
        } else if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            DelayStart();
            AudioManager.GetInstance().StartMusic(Soundtrack.VampireTheme);
            AudioManager.GetInstance().StopMusic(Soundtrack.TitleTheme);
        } else if (e.GetType() == typeof(PlayerVictoryEvent)) {
            KO();
        } else if (e.GetType() == typeof(PlayerDefeatEvent)) {
            FightOver();
        }
    }

}
