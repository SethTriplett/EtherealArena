using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class ReadyScreen : MonoBehaviour, IEventListener {

    private CanvasGroup readyGoScreen;
    private TextMeshProUGUI readyText;
    private TextMeshProUGUI goText;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }
    
    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
    }

    void Awake() {
        readyGoScreen = GetComponent<CanvasGroup>();
        readyText = transform.Find("Ready Text").GetComponent<TextMeshProUGUI>();
        goText = transform.Find("Go Text").GetComponent<TextMeshProUGUI>();
    }

    void ReadyAnimation() {
        StartCoroutine(ReadyGo());
    }

    private IEnumerator ReadyGo() {
        readyGoScreen.alpha = 1f;
        readyText.alpha = 0;
        goText.alpha = 0;

        float WAIT_DURATION = 0.5f;
        for (float x = 0; x < WAIT_DURATION; x += Time.deltaTime) {
            yield return null;
        }
        
        float READY_APPEAR_DURATION = 0.5f;
        float currentAlpha = readyText.alpha;
        for (float x = 0; x < READY_APPEAR_DURATION; x += Time.deltaTime) {
            currentAlpha = Mathf.SmoothStep(currentAlpha, 1, x / READY_APPEAR_DURATION);
            readyText.alpha = currentAlpha;
            yield return null;
        }

        float READY_ACTIVE_DURATION = 0.5f;
        for (float x = 0; x < READY_ACTIVE_DURATION; x += Time.deltaTime) {
            yield return null;
        }
        
        float READY_DISAPPEAR_DURATION = 0.5f;
        currentAlpha = readyText.alpha;
        for (float x = 0; x < READY_DISAPPEAR_DURATION; x += Time.deltaTime) {
            currentAlpha = Mathf.SmoothStep(currentAlpha, 0, x / READY_DISAPPEAR_DURATION);
            readyText.alpha = currentAlpha;
            yield return null;
        }

        float WAIT_TO_BEGIN_DURATION = 0.75f;
        for (float x = 0; x < WAIT_TO_BEGIN_DURATION; x += Time.deltaTime) {
            yield return null;
        }
        
        float GO_APPEAR_DURATION = 0.1f;
        currentAlpha = goText.alpha;
        for (float x = 0; x < GO_APPEAR_DURATION; x += Time.deltaTime) {
            currentAlpha = Mathf.SmoothStep(currentAlpha, 1, x / GO_APPEAR_DURATION);
            goText.alpha = currentAlpha;
            yield return null;
        }

        float GO_ACTIVE_DURATION = 0.5f;
        for (float x = 0; x < GO_ACTIVE_DURATION; x += Time.deltaTime) {
            yield return null;
        }
        
        float GO_DISAPPEAR_DURATION = 0.05f;
        currentAlpha = goText.alpha;
        for (float x = 0; x < GO_DISAPPEAR_DURATION; x += Time.deltaTime) {
            currentAlpha = Mathf.SmoothStep(currentAlpha, 0, x / GO_DISAPPEAR_DURATION);
            goText.alpha = currentAlpha;
            yield return null;
        }

        readyGoScreen.alpha = 0f;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            ReadyAnimation();
        }
    }

}

