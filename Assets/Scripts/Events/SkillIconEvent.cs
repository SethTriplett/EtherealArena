// Event to change the skill icon displayed
public class SkillIconEvent : IEvent {

    public int type;

    public SkillIconEvent(int type) {
        this.type = type;
    }

}