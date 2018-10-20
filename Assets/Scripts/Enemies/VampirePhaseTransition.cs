using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampirePhaseTransition : MonoBehaviour, IPhaseTransition {

    private VampireController vampireController;

    void Awake() {
        vampireController = GetComponent<VampireController>();
    }

    public void PhaseTransition(int nextPhase) {
        if (nextPhase == 2) {
            vampireController.StopAllCoroutines();
        } else if (nextPhase == 3) {
            vampireController.StopAllCoroutines();
        }
    }

    // Return the necessary stats for the boss's next phase.
    // For now, just max hp
    public int GetPhaseMaxHP(int nextPhase, int level) {
        if (nextPhase == 2) {
            return level * 5;
        } else if (nextPhase == 3) {
            return level * 5;
        }
        Debug.LogError("Wrong phase given.");
        return 0;
    }

}
