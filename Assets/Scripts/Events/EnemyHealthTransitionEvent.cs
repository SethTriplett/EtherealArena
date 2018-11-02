// Used to indicate to HUD enemy is phase transitioning
public class EnemyHealthTransitionEvent : IEvent {

    public float duration; // in seconds
    public int nextPhase; // which phase is being transitioned to

    public EnemyHealthTransitionEvent(float duration, int nextPhase) {
        this.duration = duration;
        this.nextPhase = nextPhase;
    }
    
}