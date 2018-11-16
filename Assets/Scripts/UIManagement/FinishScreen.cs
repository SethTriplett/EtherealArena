using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class FinishScreen : MonoBehaviour, IEventListener {

    private CanvasGroup finishScreen;
    private TextMeshProUGUI finishText;
    private TextMeshProUGUI winnerText;
    private TextMeshProUGUI playerText;
    private TextMeshProUGUI enemyText;

    private int phase;

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PlayerDefeatEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().SubscribeEvent(typeof(PhaseTransitionEvent), this);
    }
    
    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerVictoryEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PlayerDefeatEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(EnemyStartingDataEvent), this);
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(PhaseTransitionEvent), this);
    }

    void Awake() {
        finishScreen = GetComponent<CanvasGroup>();
        finishText = transform.Find("Finish Text").GetComponent<TextMeshProUGUI>();
        winnerText = transform.Find("Winner Text").GetComponent<TextMeshProUGUI>();
        playerText = transform.Find("Player Text").GetComponent<TextMeshProUGUI>();
        enemyText = transform.Find("Enemy Text").GetComponent<TextMeshProUGUI>();
    }

    void MatchFinish(bool playerVictory) {
        StartCoroutine(FinishSubroutine(playerVictory));
    }

    private IEnumerator FinishSubroutine(bool playerVictory) {
        finishScreen.alpha = 1f;
        finishText.alpha = 0;
        winnerText.alpha = 0;
        playerText.alpha = 0;
        enemyText.alpha = 0;

        float FINISH_APPEAR_DURATION = 0.1f;
        float currentAlpha = finishText.alpha;
        for (float x = 0; x < FINISH_APPEAR_DURATION; x += Time.deltaTime) {
            currentAlpha = Mathf.SmoothStep(currentAlpha, 1, x / FINISH_APPEAR_DURATION);
            finishText.alpha = currentAlpha;
            yield return null;
        }

        float FINISH_ACTIVE_DURATION = 1f;
        yield return new WaitForSeconds(FINISH_ACTIVE_DURATION);
        
        float FINISH_DISAPPEAR_DURATION = 0.5f;
        currentAlpha = finishText.alpha;
        for (float x = 0; x < FINISH_DISAPPEAR_DURATION; x += Time.deltaTime) {
            currentAlpha = Mathf.SmoothStep(currentAlpha, 0, x / FINISH_DISAPPEAR_DURATION);
            finishText.alpha = currentAlpha;
            yield return null;
        }

        float WAIT_TO_WINNER_DURATION = 0.25f;
        yield return new WaitForSeconds(WAIT_TO_WINNER_DURATION);
        
        float WINNER_APPEAR_DURATION = 0.5f;
        currentAlpha = winnerText.alpha;
        for (float x = 0; x < WINNER_APPEAR_DURATION; x += Time.deltaTime) {
            currentAlpha = Mathf.SmoothStep(currentAlpha, 1, x / WINNER_APPEAR_DURATION);
            winnerText.alpha = currentAlpha;
            yield return null;
        }
        
        if (playerVictory) {
            float PLAYER_APPEAR_DURATION = 0.5f;
            currentAlpha = playerText.alpha;
            for (float x = 0; x < PLAYER_APPEAR_DURATION; x += Time.deltaTime) {
                currentAlpha = Mathf.SmoothStep(currentAlpha, 1, x / PLAYER_APPEAR_DURATION);
                playerText.alpha = currentAlpha;
                yield return null;
            }

            float TEXT_ACTIVE_DURATION = 1f;
            yield return new WaitForSeconds(TEXT_ACTIVE_DURATION);
            
            EventMessanger.GetInstance().TriggerEvent(new PostBattleDialogStartEvent(phase, playerVictory));

            float TEXT_DISAPPEAR_DURATION = 0.5f;
            currentAlpha = finishText.alpha;
            for (float x = 0; x < TEXT_DISAPPEAR_DURATION; x += Time.deltaTime) {
                currentAlpha = Mathf.SmoothStep(currentAlpha, 0, x / TEXT_DISAPPEAR_DURATION);
                winnerText.alpha = currentAlpha;
                playerText.alpha = currentAlpha;
                yield return null;
            }
        } else {
            float ENEMY_APPEAR_DURATION = 0.5f;
            currentAlpha = enemyText.alpha;
            for (float x = 0; x < ENEMY_APPEAR_DURATION; x += Time.deltaTime) {
                currentAlpha = Mathf.SmoothStep(currentAlpha, 1, x / ENEMY_APPEAR_DURATION);
                enemyText.alpha = currentAlpha;
                yield return null;
            }
            
            float TEXT_ACTIVE_DURATION = 1f;
            yield return new WaitForSeconds(TEXT_ACTIVE_DURATION);

            EventMessanger.GetInstance().TriggerEvent(new PostBattleDialogStartEvent(phase, playerVictory));

            float TEXT_DISAPPEAR_DURATION = 0.5f;
            currentAlpha = finishText.alpha;
            for (float x = 0; x < TEXT_DISAPPEAR_DURATION; x += Time.deltaTime) {
                currentAlpha = Mathf.SmoothStep(currentAlpha, 0, x / TEXT_DISAPPEAR_DURATION);
                winnerText.alpha = currentAlpha;
                enemyText.alpha = currentAlpha;
                yield return null;
            }
        }

        finishScreen.alpha = 0f;
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(PlayerVictoryEvent)) {
            MatchFinish(true);
        } else if (e.GetType() == typeof(PlayerDefeatEvent)) {
            MatchFinish(false);
        } else if (e.GetType() == typeof(EnemyStartingDataEvent)) {
            phase = 1;
            EnemyStartingDataEvent enemyDataEvent = e as EnemyStartingDataEvent;
            if (enemyText != null) {
                enemyText.text = enemyDataEvent.name;
            }
        } else if (e.GetType() == typeof(PhaseTransitionEvent)) {
            PhaseTransitionEvent phaseTransitionEvent = e as PhaseTransitionEvent;
            phase = phaseTransitionEvent.nextPhase;
        }
    }

}

