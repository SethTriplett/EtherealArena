using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseLightBow : MonoBehaviour, ISkill {

    [SerializeField] private Sprite[] bowSprites;
    [SerializeField] private GameObject bow;
    private SpriteRenderer bowRenderer;
    private GameObject arrow;
    [SerializeField] private GameObject arrowPrefab;
    private int arrowIndex;
    private float chargeTime;

    void Awake() {
        bowRenderer = bow.GetComponent<SpriteRenderer>();
    }

    void Start() {
        arrowIndex = ObjectPooler.instance.GetIndex(arrowPrefab);
    }

    public void UseSkill(Transform hand) {
        // Get arrow if necessary
        if (chargeTime == 0f || arrow == null) {
            arrow = ObjectPooler.instance.GetDanmaku(arrowIndex);
            LightArrow arrowScript = arrow.GetComponent<LightArrow>();
            arrowScript.SetCharging();
            arrowScript.SetOwner(gameObject);
            arrow.SetActive(true);
            AudioManager.GetInstance().PlaySound(Sound.Charge);
        }

        // Set bow transform
        bow.transform.rotation = hand.transform.rotation;
        Vector3 bowPos = hand.TransformPoint(0.275f, 0, 0);
        bow.transform.position = bowPos;

        // charge bow
        chargeTime += Time.deltaTime;

        // Set bow charge
        int chargeLevel = Mathf.FloorToInt(chargeTime / 0.25f);
        if (chargeLevel >= bowSprites.Length) {
            chargeLevel = bowSprites.Length - 1;
        }
        bowRenderer.sprite = bowSprites[chargeLevel];

        // Set arrow charge level
        LightArrow arrowScriptAgain = arrow.GetComponent<LightArrow>();
        arrowScriptAgain.SetChargeLevel(chargeLevel);

        // set arrow position based on charge level
        arrow.transform.rotation = bow.transform.rotation;
        Vector3 arrowPos = bow.transform.TransformPoint(FindArrowOffset(chargeLevel), 0, 0);
        arrow.transform.position = arrowPos;
    }

    public void AimSkill(Transform hand) {
        // Set bow transform
        bow.transform.rotation = hand.transform.rotation;
        Vector3 bowPos = hand.TransformPoint(0.275f, 0, 0);
        bow.transform.position = bowPos;
    }

    public void ReleaseSkill() {
        if (arrow != null) {
            LightArrow arrowScript = arrow.GetComponent<LightArrow>();
            arrowScript.SetReleased();
        }
        chargeTime = 0f;
        bowRenderer.sprite = bowSprites[0];
        AudioManager.GetInstance().PlaySound(Sound.BowRelease);
        AudioManager.GetInstance().StopSound(Sound.Charge);
    }

    public void SetActiveSkill() {
        bow.SetActive(true);
    }

    public void SetInactiveSkill() {
        bow.SetActive(false);
        AudioManager.GetInstance().StopSound(Sound.Charge);
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

}
