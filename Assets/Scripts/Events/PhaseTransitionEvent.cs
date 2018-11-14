// Event used to signal a transition to a new phase
public class PhaseTransitionEvent : IEvent {

    public int nextPhase;

    public PhaseTransitionEvent(int nextPhase) {
        this.nextPhase = nextPhase;
    }

}