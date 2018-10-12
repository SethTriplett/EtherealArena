// This event is passed to an enemy when it's loading to give it necessary information
public class EnemyStartingDataEvent : IEvent {

    public int level;

    public EnemyStartingDataEvent(int level) {
        this.level = level;
    }

}