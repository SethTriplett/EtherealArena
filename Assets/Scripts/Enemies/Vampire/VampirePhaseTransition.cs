using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampirePhaseTransition : MonoBehaviour, IPhaseTransition {

    private VampireController vampireController;

    void Awake() {
        vampireController = GetComponent<VampireController>();
    }

    public void PhaseTransition(int nextPhase) {
        vampireController.StopFunction();
        if (nextPhase == 2) {
            vampireController.StopAllCoroutines();
        } else if (nextPhase == 3) {
            vampireController.StopAllCoroutines();
        }
    }

    // Return the necessary stats for the boss's next phase.
    // For now, just max hp
    public int GetPhaseMaxHP(int nextPhase, int level, int maxPhase) {
        float totalHealth = 5 * (level + 1);
        if (nextPhase == 2) {
            if (maxPhase == 2) {
                return Mathf.FloorToInt(totalHealth * 6 / 10f);
            } else if (maxPhase == 3) {
                return Mathf.FloorToInt(totalHealth * 3 / 10f);
            }
        } else if (nextPhase == 3) {
            return Mathf.FloorToInt(totalHealth * 5 / 10f);
        }
        Debug.LogError("Wrong phase given.");
        return 0;
    }

}
