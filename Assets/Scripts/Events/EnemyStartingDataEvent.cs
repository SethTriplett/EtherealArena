// This event is passed to an enemy when it's loading to give it necessary information
public class EnemyStartingDataEvent : IEvent {

    public int level;
    
    // Number of phases in the boss. If 0, will be determined by level
    public int maxPhase;

    public string name;

    public EnemyStartingDataEvent(int level, int maxPhase, EnemyType enemyType) {
        this.level = level;
        this.maxPhase = maxPhase;
        switch(enemyType) {
            case EnemyType.Dummy:
                this.name = "Dummy";
                break;
            case EnemyType.Vampire:
                this.name = "Versum";
                break;
            case EnemyType.Psychic:
                this.name = "Celif";
                break;
            case EnemyType.DarkPlayer:
                this.name = "Sachure";
                break;
        }
    }

}