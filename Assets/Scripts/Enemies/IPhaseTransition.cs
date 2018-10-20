// Interfaces parenting phase transition classes for bosses
public interface IPhaseTransition {

    // Method used to drive the boss's transition
    void PhaseTransition(int nextPhase);

    // Return HP for the next phase based on phase and level
    int GetPhaseMaxHP(int nextPhase, int level);

}