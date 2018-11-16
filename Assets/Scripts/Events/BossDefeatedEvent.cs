public class BossDefeatedEvent : IEvent {

    public EnemyType type;
    public int phase;

    public BossDefeatedEvent(EnemyType type, int phase) {
        this.type = type;
        this.phase = phase;
    }

}