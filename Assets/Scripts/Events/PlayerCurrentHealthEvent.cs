// Event communicating the player's current health to the UI
public class PlayerCurrentHealthEvent : IEvent {

    public int currentHealth;

    public PlayerCurrentHealthEvent(int health) {
        this.currentHealth = health;
    }

}