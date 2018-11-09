using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBow : MonoBehaviour, IPoolable, IEventListener {

    [SerializeField] private Sprite[] bowSprites;
    private SpriteRenderer bowRenderer;
    private GameObject arrow;
    [SerializeField] private GameObject arrowPrefab;
    private int darkArrowIndex;
    private bool charging = false;
    private GameObject owner;
    private const float BASE_CHARGE_TIME = 1.5f;

    void Awake() {
        bowRenderer = GetComponent<SpriteRenderer>();
        darkArrowIndex = ObjectPooler.instance.GetIndex(arrowPrefab);
        if (darkArrowIndex == -1) {
            Debug.LogError("Dark Arrow index not found");
        }
        charging = false;
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(DeleteAttacksEvent), this);
    }
    
    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(DeleteAttacksEvent), this);
        charging = false;
    }

    public void SetOwner(GameObject owner) {
        this.owner = owner;
    }

    private void NockArrow() {
        arrow = ObjectPooler.instance.GetDanmaku(darkArrowIndex);
        if (arrow == null) {
            Debug.LogError("No arrow");
        }
        DarkArrow darkArrowScript = arrow.GetComponent<DarkArrow>();
        //arrow.transform.rotation = transform.rotation;
        arrow.transform.position = transform.TransformPoint(FindArrowOffset(0), 0f, 0f);
        darkArrowScript.SetCharging();
        arrow.SetActive(true);
    }

    public IEnumerator AutoChargeAndFire() {
        if (arrow == null) NockArrow();
        DarkArrow darkArrowScript = arrow.GetComponent<DarkArrow>();

        charging = true;

        float timer = BASE_CHARGE_TIME;
        while (timer > 0) {
            // Increase charge level over time
            int chargeLevel = Mathf.FloorToInt(bowSprites.Length * (1 - (timer / BASE_CHARGE_TIME)));
            // Special case
            if (chargeLevel >= bowSprites.Length) chargeLevel = bowSprites.Length - 1;
            bowRenderer.sprite = bowSprites[chargeLevel];

            // set arrow position based on charge level
            darkArrowScript.SetChargeLevel(chargeLevel);
            arrow.transform.rotation = transform.rotation;
            Vector3 arrowPos = transform.TransformPoint(FindArrowOffset(chargeLevel), 0, 0);
            arrow.transform.position = arrowPos;

            yield return null;
            timer -= Time.deltaTime;
        }
        Fire();
        charging = false;
    }

    private void Fire() {
        DarkArrow darkArrowScript = arrow.GetComponent<DarkArrow>();
        darkArrowScript.SetReleased();
        arrow = null;
        bowRenderer.sprite = bowSprites[0];
    }

    public bool IsBusy() {
        return charging;
    }

    public void Deactivate() {
        StopAllCoroutines();
        gameObject.SetActive(false);
        if (arrow) {
            arrow.SetActive(false);
        }
    }

    private float FindArrowOffset(int chargeLevel) {
        switch(chargeLevel) {
            case 0:
                return 0.225f;
            case 1:
                return 0.175f;
            case 2:
                return 0.125f;
            case 3:
                return 0.085f;
            case 4:
                return 0.025f;
            case 5:
                return 0.015f;
        }
        return 0.225f;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(DeleteAttacksEvent)) {
            DeleteAttacksEvent deleteAttacksEvent = e as DeleteAttacksEvent;
            if (deleteAttacksEvent.owner == owner) {
                StopAllCoroutines();
                Deactivate();
            }
        }
    }

}
