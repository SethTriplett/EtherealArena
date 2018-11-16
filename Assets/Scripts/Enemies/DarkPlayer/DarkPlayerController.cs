using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkPlayerController : MonoBehaviour, IEventListener {

    private enum DarkPlayerState {
        ChoosingAttack, PreppingAttack, Attacking, DoNothing
    }
    private DarkPlayerState state;

    private int phase;
    [SerializeField] private Transform hand;

    private SpriteRenderer bodyRenderer;
    private Transform arm;
    private SpriteRenderer armRenderer;
    private Transform head;
    private SpriteRenderer headRenderer;
    private Transform Satoration;
    private SpriteRenderer SatorationRenderer;
    private Animator animator;

    private bool facingRight = true;

    private const int SHOTS_PER_WAVE = 9;
    private const float STORM_SPREAD_ANGLE = 135f;

    [SerializeField] private GameObject spiritShotPrefab;
    [SerializeField] private GameObject darkBowPrefab;
    [SerializeField] private GameObject darkFlamePrefab;
    private int spiritShotIndex;
    private int darkBowIndex;
    private int darkFlameIndex;

    private float cooldownTimer = 5f;
    private int preppedAttack;

    private float warpTimer;

    private const float WARP_DURATION = 1f;
    private readonly Vector3[] positions = {
        new Vector3(0f, 0f, 0),
        new Vector3(6.2f, 0f, 0),
        new Vector3(6.2f, 3.8f, 0),
        new Vector3(0f, 3.8f, 0),
        new Vector3(-6.2f, 3.8f, 0),
        new Vector3(-6.2f, 0f, 0),
        new Vector3(-6.2f, -3.8f, 0),
        new Vector3(0f, -3.8f, 0),
        new Vector3(6.2f, -3.8f, 0),
    };

    void Start() {
        spiritShotIndex = ObjectPooler.instance.GetIndex(spiritShotPrefab);
        darkBowIndex = ObjectPooler.instance.GetIndex(darkBowPrefab);
        darkFlameIndex = ObjectPooler.instance.GetIndex(darkFlamePrefab);

        state = DarkPlayerState.ChoosingAttack;

        bodyRenderer = GetComponent<SpriteRenderer>();
        arm = transform.Find("DarkPlayer_Arm");
        armRenderer = arm.GetComponent<SpriteRenderer>();
        head = transform.Find("DarkPlayer_Head");
        headRenderer = head.GetComponent<SpriteRenderer>();
        Satoration = transform.Find("Satoration");
        SatorationRenderer = Satoration.GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PhaseTransitionEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerVictoryEvent), this);
    }

    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PhaseTransitionEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerVictoryEvent), this);

        EventMessanger.GetInstance().TriggerEvent(new DeleteAttacksEvent(gameObject));
    }

    void Update() {
        if (state == DarkPlayerState.ChoosingAttack) {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0) {
                if (phase == 1) {
                    float randomAttack = Random.Range(0f, 1f);
                    if (randomAttack < 0.8f) {
                        preppedAttack = 0;
                        // Choose one of the corners
                        int randomPosition = Random.Range(1, 4);
                        randomPosition *= 2;
                        StartCoroutine(Warp(positions[randomPosition]));
                        cooldownTimer = 4f;
                    } else {
                        preppedAttack = 1;
                        StartCoroutine(Warp(positions[0]));
                        cooldownTimer = 3f;
                    }
                } else if (phase == 2) {
                    float randomAttack = Random.Range(0f, 1f);
                    if (randomAttack < 0.5f) {
                        preppedAttack = 2;
                        int randomPosition = Random.Range(2, 4);
                        StartCoroutine(Warp(positions[randomPosition]));
                        cooldownTimer = 3f;
                    } else {
                        preppedAttack = 3;
                        int randomPosition = Random.Range(6, 8);
                        StartCoroutine(Warp(positions[randomPosition]));
                        cooldownTimer = 3f;
                    }
                } else if (phase == 3) {
                    preppedAttack = 4;
                    int randomPosition = Random.Range(1, 4);
                    randomPosition *= 2;
                    randomPosition += 1;
                    StartCoroutine(Warp(positions[randomPosition]));
                    cooldownTimer = 5f;
                }
                state = DarkPlayerState.PreppingAttack;
            }
        } else if (state == DarkPlayerState.PreppingAttack) {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer < 0) {
                if (preppedAttack == 0) {
                    if (transform.position.y > 0) {
                        if (transform.position.x > 0) {
                            StartCoroutine(Storm(10, -135));
                        } else {
                            StartCoroutine(Storm(10, -45));
                        }
                    } else {
                        if (transform.position.x > 0) {
                            StartCoroutine(Storm(10, 135));
                        } else {
                            StartCoroutine(Storm(10, 45));
                        }
                    }
                } else if (preppedAttack == 1) {
                    StartCoroutine(EndlessPursuit());
                } else if (preppedAttack == 2) {
                    StartCoroutine(CrossFire());
                } else if (preppedAttack == 3) {
                    StartCoroutine(UnholyImmolation());
                } else if (preppedAttack == 4) {
                    StartCoroutine(ArcherStorm());
                }
                state = DarkPlayerState.Attacking;
            }
        }

        if (phase == 3 && state == DarkPlayerState.Attacking) {
            warpTimer -= Time.deltaTime;
            if (warpTimer < 0f) {
                int randomPos = Random.Range(1, 8);
                Mathf.FloorToInt((randomPos + 1) / 2f);
                StartCoroutine(Warp(positions[randomPos]));
                warpTimer = 7f;
            }
        }

        // Face toward the center
        if (transform.position.x < -1) {
            facingRight = true;
        } else if (transform.position.x > 1) {
            facingRight = false;
        }

        if (!facingRight) {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            bodyRenderer.flipY = true;
            armRenderer.flipY = true;
            headRenderer.flipY = true;
            SatorationRenderer.flipY = true;
            if (arm.localPosition.y > 0) {
                arm.localPosition = new Vector3(arm.localPosition.x, -arm.localPosition.y, arm.localPosition.z);
            }
            if (head.localPosition.y > 0) {
                head.localPosition = new Vector3(head.localPosition.x, -head.localPosition.y, head.localPosition.z);
            }
            if (Satoration.localPosition.y > 0) {
                Satoration.localPosition = new Vector3(Satoration.localPosition.x, -Satoration.localPosition.y, Satoration.localPosition.z);
            }
         } else {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            bodyRenderer.flipY = false;
            armRenderer.flipY = false;
            headRenderer.flipY = false;
            SatorationRenderer.flipY = false;
            if (arm.localPosition.y < 0) {
                arm.localPosition = new Vector3(arm.localPosition.x, -arm.localPosition.y, arm.localPosition.z);
            }
            if (head.localPosition.y < 0) {
                head.localPosition = new Vector3(head.localPosition.x, -head.localPosition.y, head.localPosition.z);
            }
            if (Satoration.localPosition.y < 0) {
                Satoration.localPosition = new Vector3(Satoration.localPosition.x, -Satoration.localPosition.y, Satoration.localPosition.z);
            }
        }
    }

    private void TransitionPhase(int nextPhase) {
        StopAllCoroutines();
        state = DarkPlayerState.ChoosingAttack;
        cooldownTimer = 1f;
        arm.localEulerAngles = new Vector3(0, 0, 0);
        EventMessanger.GetInstance().TriggerEvent(new DeleteAttacksEvent(gameObject));
        if (nextPhase == 1) {
            phase = 1;
        } else if (nextPhase == 2) {
            phase = 2;
        } else if (nextPhase == 3) {
            phase = 3;
        }
    }

    private void KO() {
        StopAllCoroutines();
        arm.localEulerAngles = new Vector3(0, 0, 0);
        state = DarkPlayerState.DoNothing;
        EventMessanger.GetInstance().TriggerEvent(new DeleteAttacksEvent(gameObject));
    }

    private IEnumerator Warp(Vector3 destination) {
        animator.SetTrigger("WarpOut");
        AudioManager.GetInstance().PlaySound(Sound.WarpOut);
        yield return new WaitForSeconds(WARP_DURATION);
        transform.position = destination;
        animator.SetTrigger("WarpIn");
        AudioManager.GetInstance().PlaySound(Sound.WarpIn);
        yield return null;
        animator.SetTrigger("Idle");
    }

    private IEnumerator Storm(int waves, float aimedAngle) {
        if (Mathf.Abs(transform.position.x) < 4 || Mathf.Abs(transform.position.y) < 3) Debug.LogWarning("Close to center.");
        arm.rotation = Quaternion.Euler(0f, 0f, aimedAngle);
        for (int x = 0; x < waves; x++) {
            // Alternate patterns
            if (x % 2 == 0) {
                for (int y = 0; y < SHOTS_PER_WAVE; y++) {
                    GameObject spiritShot = ObjectPooler.instance.GetDanmaku(spiritShotIndex);
                    SpiritShotEnemy spiritShotScript = spiritShot.GetComponent<SpiritShotEnemy>();
                    spiritShot.transform.position = hand.position;
                    spiritShotScript.SetOwner(gameObject);
                    // Orient each shot
                    spiritShot.transform.rotation = Quaternion.Euler(0, 0, aimedAngle - (STORM_SPREAD_ANGLE / 2f) + (STORM_SPREAD_ANGLE * (y / (float) (SHOTS_PER_WAVE - 1))));
                    spiritShot.SetActive(true);
                }
            } else {
                float alternatingOffset = STORM_SPREAD_ANGLE / (2 * SHOTS_PER_WAVE);
                for (int y = 0; y < SHOTS_PER_WAVE + 1; y++) {
                    GameObject spiritShot = ObjectPooler.instance.GetDanmaku(spiritShotIndex);
                    SpiritShotEnemy spiritShotScript = spiritShot.GetComponent<SpiritShotEnemy>();
                    spiritShot.transform.position = hand.position;
                    spiritShotScript.SetOwner(gameObject);
                    // Orient each shot
                    spiritShot.transform.rotation = Quaternion.Euler(0, 0, aimedAngle - (STORM_SPREAD_ANGLE / 2f) + (STORM_SPREAD_ANGLE * (y / (float) (SHOTS_PER_WAVE - 1))) - alternatingOffset);
                    spiritShot.SetActive(true);
                }
            }
            AudioManager.GetInstance().PlaySound(Sound.Schff);
            yield return new WaitForSeconds(0.4f);
        }
        arm.localEulerAngles = new Vector3(0, 0, 0);
        cooldownTimer = 4f;
        state = DarkPlayerState.ChoosingAttack;
    }

    private IEnumerator EndlessPursuit() {
        if (transform.position != Vector3.zero) Debug.LogWarning("Wrong position");
        float angle = 90f;
        float duration = 10f;

        while (duration > 0f) {
            facingRight = angle <= 90 || angle > 270;
            arm.rotation = Quaternion.Euler(0, 0, angle);
            GameObject shot = ObjectPooler.instance.GetDanmaku(spiritShotIndex);
            SpiritShotEnemy spiritShotScript = shot.GetComponent<SpiritShotEnemy>();
            spiritShotScript.SetOwner(gameObject);
            shot.transform.position = hand.position;
            shot.transform.rotation = hand.rotation;
            shot.SetActive(true);

            AudioManager.GetInstance().PlaySound(Sound.Schff);
            yield return new WaitForSeconds(0.1f);
            duration -= 0.1f;

            angle -= 10f;
            if (angle < 0) angle += 360f;
            angle %= 360f;
        }
        arm.localEulerAngles = new Vector3(0, 0, 0);
        cooldownTimer = 0.5f;
        state = DarkPlayerState.ChoosingAttack;
    }

    private void Arrow() {
        GameObject darkBow = ObjectPooler.instance.GetDanmaku(darkBowIndex);
        DarkBow darkBowScript = darkBow.GetComponent<DarkBow>();
        darkBowScript.SetOwner(gameObject);
        darkBow.transform.position = new Vector3(transform.position.x + 1.5f, transform.position.y + Random.Range(-1f, 1f), 0f);
        darkBow.SetActive(true);
        StartCoroutine(darkBowScript.AutoChargeAndFire());
    }

    private IEnumerator ArcherStorm() {

        // Set up
        const int NUM_BOWS = 28;

        Vector3[] positions = new Vector3[NUM_BOWS];
        // calculate position based on number of bows
        for (int x = 0; x < NUM_BOWS; x++) {
            // find y position
            float yLevel = Mathf.FloorToInt(x / 2) - (NUM_BOWS / 4f) + .5f;
            float height = yLevel * 0.75f;
            // give x position based on x
            if (x % 4 == 0) {
                positions[x] = new Vector3(-7f, height, 0f);
            } else if (x % 4 == 1) {
                positions[x] = new Vector3(7f, height, 0f);
            } else if (x % 4 == 2) {
                positions[x] = new Vector3(-6.5f, height, 0f);
            } else if (x % 4 == 3) {
                positions[x] = new Vector3(6.5f, height, 0f);
            }
        }

        DarkBow[] bows = new DarkBow[NUM_BOWS];
        for (int x = 0; x < NUM_BOWS; x++) {
            GameObject bow = ObjectPooler.instance.GetDanmaku(darkBowIndex);
            if (bow == null) {
                Debug.LogError("Missing bow");
            }
            DarkBow bowScript = bow.GetComponent<DarkBow>();
            if (bowScript != null) {
                bows[x] = bowScript;
            } else {
                Debug.LogError("No DarkBow script.");
            }
            bowScript.SetOwner(gameObject);
            bow.transform.position = positions[x];
            if (bow.transform.position.x > 0) {
                bow.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            } else {
                bow.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            bow.SetActive(true);
        }

        // Attack portion
        const float DURATION = 600f;
        const float COOLDOWN = 0.5f;

        for (int x = 0; x < DURATION / COOLDOWN; x++) {
            int randIndex;
            int attemptLimit = NUM_BOWS;
            bool attacked = false;
            do {
                randIndex = Random.Range(0, NUM_BOWS);
                if (!bows[randIndex].IsBusy()) {
                    StartCoroutine(bows[randIndex].AutoChargeAndFire());
                    attacked = true;
                }
                attemptLimit--;
            } while (!attacked && attemptLimit > 0);
            yield return new WaitForSeconds(0.5f);
        }

        // Cleanup
        for (int x = 0; x < NUM_BOWS; x++) {
            bows[x].Deactivate();
        }

        cooldownTimer = 10f;

    }

    private IEnumerator CrossFire() {
        const int NUM_FLAMES = 10;

        for (int x = 0; x < NUM_FLAMES; x++) {
            float xLevel = Mathf.FloorToInt(x / 2) - (NUM_FLAMES / 4f) + 0.5f;
            float xPos = xLevel * 3f;

            GameObject darkFlame = ObjectPooler.instance.GetDanmaku(darkFlameIndex);
            if (darkFlame == null) Debug.LogError("Dark flame not found in pool.");
            DarkFlame darkFlameScript = darkFlame.GetComponent<DarkFlame>();
            if (darkFlameScript == null) Debug.LogError("Dark flame script not found");
            darkFlameScript.SetOwner(gameObject);
            darkFlame.transform.position = new Vector3(xPos, 5f, 0f);
            if (x % 2 == 0) {
                darkFlameScript.SetFalling(2f, 1.5f);
            } else {
                darkFlameScript.SetFalling(2f, -1.5f);
            }
            darkFlame.SetActive(true);
        }

        yield return null;
        cooldownTimer = 3f;
        state = DarkPlayerState.ChoosingAttack;
    }

    private IEnumerator UnholyImmolation() {
        const int NUM_FLAMES = 11;
        Vector3 CENTER_POINT = Vector3.zero;
        float CONVERGE_SPEED = 1.2f;
        float ANGULAR_SPEED = 1.8f;

        // Set up
        GameObject[] flames = new GameObject[NUM_FLAMES];
        float randomOffset = Random.Range(0f, 2 * Mathf.PI);
        for (int x = 0; x < NUM_FLAMES; x++) {
            GameObject flame = ObjectPooler.instance.GetDanmaku(darkFlameIndex);
            if (flame == null) Debug.LogError("Dark flame not found in pool.");
            flames[x] = flame;
            DarkFlame darkFlameScript = flame.GetComponent<DarkFlame>();
            if (darkFlameScript == null) Debug.LogError("Dark flame script not found.");
            darkFlameScript.SetOwner(gameObject);
            darkFlameScript.SetIdle();

            float xPos = 9 * Mathf.Sin((x * 2 * Mathf.PI / (NUM_FLAMES + 1)) + randomOffset);
            float yPos = 9 * Mathf.Cos(x * 2 * Mathf.PI / (NUM_FLAMES + 1) + randomOffset);
            flame.transform.position = new Vector3(xPos, yPos, 0f);

            flame.SetActive(true);
        }

        yield return null;

        // Attack
        float timeOut = 20f;
        bool converged = false;
        while (!converged && timeOut > 0) {
            for (int x = 0; x < NUM_FLAMES; x++) {
                GameObject flame = flames[x];
                Vector3 distanceVector = flame.transform.position - CENTER_POINT;
                flame.transform.position -= distanceVector.normalized * CONVERGE_SPEED * Time.deltaTime;
                float distance = distanceVector.magnitude - CONVERGE_SPEED * Time.deltaTime;
                if (distance < 0.05f) converged = true;
                float angle = Vector3.Angle(distanceVector, Vector3.right);
                if (distanceVector.y < 0) angle = -1 * Mathf.Abs(angle);
                angle *= Mathf.Deg2Rad;
                angle -= ANGULAR_SPEED * Time.deltaTime;
                flame.transform.position = new Vector3(distance * Mathf.Cos(angle), distance * Mathf.Sin(angle), 0f);
            }
            yield return null;
            timeOut -= Time.deltaTime;
        }

        // cleanup
        for (int x = 0; x < NUM_FLAMES; x++) {
            if (flames[x].activeSelf) {
                DarkFlame darkFlame = flames[x].GetComponent<DarkFlame>();
                darkFlame.Deactivate();
            }
        }
        cooldownTimer = 5f;
        state = DarkPlayerState.ChoosingAttack;
    }
    
    private void DelayStart() {
        float duration = 2.75f;
        StartCoroutine(DelayStartSubroutine(duration));
    }

    private IEnumerator DelayStartSubroutine(float duration) {
        while (duration > 0) {
            duration -= Time.deltaTime;
            yield return null;
        }
        TransitionPhase(1);
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PhaseTransitionEvent)) {
            PhaseTransitionEvent phaseTransitionEvent = e as PhaseTransitionEvent;
            TransitionPhase(phaseTransitionEvent.nextPhase);
        } else if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            AudioManager.GetInstance().StartMusic(Soundtrack.FinalBossTheme);
            AudioManager.GetInstance().StopMusic(Soundtrack.TitleTheme);
            DelayStart();
        } else if (e.GetType() == typeof(PlayerVictoryEvent)) {
            KO();
        }
    }

}
