using EtherealArena.WorldMap;

public class WorldMapMovementEvent : IEvent {

    public WorldMapNode currentNode;

    public WorldMapMovementEvent(WorldMapNode currentNode) {
        this.currentNode = currentNode;
    }

}