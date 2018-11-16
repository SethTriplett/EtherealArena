public class WorldMapMenuEvent : IEvent {

    public bool paused;

    public WorldMapMenuEvent(bool paused) {
        this.paused = paused;
    }

}