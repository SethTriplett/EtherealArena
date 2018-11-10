
public class BackgroundLoadEvent : IEvent {

    public BackgroundEnum background;

    public BackgroundLoadEvent(BackgroundEnum background) {
        this.background = background;
    }

}