using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : MonoBehaviour {

    private ObjectPooler bloodPooler;
    [SerializeField]
    private Transform playerTransform;
    private float attackingTimer = 0f;
    private bool stopAttacking = false;
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
    private bool grabbed;


    // Use this for initialization
    void Start () {
		bloodPooler = ObjectPooler.sharedPooler;
        curMeth = "attack";
        timer = -1f;
        attacked = true;
        right = true;
        GAS = 2;
        grabbed = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(timer <= 0)
        {
            setTimer();
        }
        timer -= Time.deltaTime;
        if (curMeth.Equals("move"))
        {
            move();
        }
        else if (curMeth.Equals("attack") && !attacked)
        {
            attack();
            attacked = true;
        }
        else if (curMeth.Equals("AG"))
        {
            GrabAttack(playerTransform.position);
            attacked = true;
        }
        if(playerTransform.position.x < transform.position.x && right)
        {
            faceLeft();
        }
        else if(playerTransform.position.x > transform.position.x && !right)
        {
            faceRight();
        }
	}


    private void setTimer()
    {
        grabbed = false;
        hand.GetComponent<HandAttack>().release();
        if(!attacked)
        {
            timer = 10f;
            Debug.Log("attack");
            if(Random.Range(0, 2) == 0)
            {
                curMeth = "AG";
                timer = 5f;
            }
            else
            {
                curMeth = "attack";
            }
        } 
        else
        {
            timer = 5f;
            attacked = false;
            curMeth = "move";
            target = new Vector3(Random.Range(-8.3f + .38f, 8.2f - .38f), Random.Range(-5.4f + .64f, 5.4f - .64f));
            moveVec = Vector3.Normalize(target - transform.position);
        }
        hand.SetActive(false);
    }
    

    private void move()
    {
        if (!(Mathf.Abs(target.x - transform.position.x) < .1) && !(Mathf.Abs(target.y - transform.position.y) < .1))
        {
            transform.position += moveVec * Time.deltaTime * speed;
        }
    }

    private void attack()
    {
        if (!secondForm)
        {
            StartCoroutine(CircleHell(transform.position, 4, .5f, 4, playerTransform.position));
        } 
        else
        {
            StartCoroutine(CircleHell(transform.position, 5, .5f, 5, playerTransform.position));
        }
    }

    IEnumerator CircleHell(Vector3 centerPoint, int number, float radius, int numOfCircles, Vector3 playerLoc)
    {
        for (int j = 0; j < numOfCircles; j++)
        {
            float angleTar = Mathf.Atan2((playerLoc.y - transform.position.y), (playerLoc.x - transform.position.x));
            target = new Vector3(Mathf.Cos(angleTar - (Mathf.PI / 2) + (Mathf.PI * j / numOfCircles)), Mathf.Sin(angleTar - (Mathf.PI / 2) + (Mathf.PI * j / numOfCircles)));
            for (int i = 0; i < number; i++)
            {
                float angle = (-i / (float)number) * 2 * Mathf.PI + Mathf.PI / 2;
                float xPos = centerPoint.x + radius * Mathf.Cos(angle);
                float yPos = centerPoint.y + radius * Mathf.Sin(angle);
                GameObject knife = bloodPooler.GetDanmaku(1);
                if (knife != null)
                {
                    knife.AddComponent<BloodBullet>();
                    knife.GetComponent<Knife>().enabled = false;
                    BloodBullet bloodBulletScript = knife.GetComponent<BloodBullet>();
                    knife.transform.position = new Vector3(xPos, yPos, 0f);
                    bloodBulletScript.setTarget(target);
                    bloodBulletScript.setAttackOne();
                    bloodBulletScript.setSpeed(j + 3);
                    knife.SetActive(true);
                }
                //yield return new WaitForSeconds(2f / number);
            }
            yield return new WaitForSeconds(1);
        }
    }
    /* WIP for another attack
    IEnumerator BloodBolts(int number, Vector3 playerLoc)
    {
        float angleTar = Mathf.Atan2((playerLoc.y - transform.position.y), (playerLoc.x - transform.position.x));
        for(int i = 0; i < number; i++)
        {

        }
    }
    */


    void GrabAttack(Vector3 playerLoc)
    {
        //Debug.Log("Grab attack");
        hand.SetActive(true);
        float angleTar = Mathf.Atan2((playerLoc.y - transform.position.y), (playerLoc.x - transform.position.x));
        target = new Vector3(playerLoc.x - transform.position.x, playerLoc.y - transform.position.y);
        target = Vector3.Normalize(target);
        if (grabbed)
        {
            Vector3 toCenter = new Vector3(-transform.position.x, -transform.position.y);
            toCenter = Vector3.Normalize(toCenter);
            if (!(Mathf.Abs(transform.position.x) < .1f) && !(Mathf.Abs(transform.position.y) < .1f))
            {
                transform.position = transform.position + toCenter * Time.deltaTime * 5;
            }
        }
        else
        {
            transform.position = transform.position + target * GAS * Time.deltaTime;
        }
    }

    private void faceLeft()
    {
        GetComponent<SpriteRenderer>().flipX = true;
        hand.transform.position = hand.transform.position + new Vector3(-2, 0, 0);
        right = false;
    }

    private void faceRight()
    {
        GetComponent<SpriteRenderer>().flipX = false;
        hand.transform.position = hand.transform.position + new Vector3(2, 0, 0);
        right = true;
    }

    public void playerGrabbed()
    {
        grabbed = true;
    }
}
