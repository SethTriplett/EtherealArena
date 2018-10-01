// Tells the UI how much energy the player has now.
public class PlayerCurrentEnergyEvent : IEvent {

    public float energy;

    public PlayerCurrentEnergyEvent(float energy) {
        this.energy = energy;
    }

}