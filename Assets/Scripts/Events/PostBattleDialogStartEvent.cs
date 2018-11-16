// Event to trigger post battle banter
public class PostBattleDialogStartEvent : IEvent {

    public int phase;
    public bool playerVictory;

    public PostBattleDialogStartEvent(int phase, bool playerVictory) {
        this.phase = phase;
        this.playerVictory = playerVictory;
    }

}