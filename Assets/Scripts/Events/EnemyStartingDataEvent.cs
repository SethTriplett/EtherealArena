// This event is passed to an enemy when it's loading to give it necessary information
public class EnemyStartingDataEvent : IEvent {

    public int level;
    
    // Number of phases in the boss. If 0, will be determined by level
    public int maxPhase;

    public string name;

    public EnemyStartingDataEvent(int level, int maxPhase, string name) {
        this.level = level;
        this.maxPhase = maxPhase;
        this.name = name;
    }

}