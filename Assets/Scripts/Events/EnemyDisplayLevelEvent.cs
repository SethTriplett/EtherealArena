// Used to tell the UI what the enemy's level is
public class EnemyDisplayLevelEvent : IEvent {

    public int level;

    public EnemyDisplayLevelEvent(int level) {
        this.level = level;
    }

}