public class PlayAudioEvent: IEvent {
    
    public Sound sound;

    public PlayAudioEvent(Sound sound) {
        this.sound = sound;
    }

}