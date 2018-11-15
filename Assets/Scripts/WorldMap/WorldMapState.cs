using EtherealArena.WorldMap;

public class WorldMapState {

    private WorldMapNode currentNode;
    private bool[] edgesOpen;
    private bool[] bossesDefeated;

    void Initialize() {
        currentNode = null;
        edgesOpen = new bool[9];
        bossesDefeated = new bool[9];
    }

    public void SetCurrentNode(WorldMapNode node) {
        this.currentNode = node;
    }
    
    public WorldMapNode GetCurrentNode() {
        return this.currentNode;
    }

    public void SetEdgeOpen(int index, bool isOpen) {
        if (index < edgesOpen.Length) {
            edgesOpen[index] = isOpen;
        }
    }

    public bool GetEdgeOpen(int index) {
        if (index < edgesOpen.Length) {
            return edgesOpen[index];
        } else return false;
    }

    public void SetBossDefeated(int index, bool isDefeated) {
        if (index < bossesDefeated.Length) {
            bossesDefeated[index] = isDefeated;
        }
    }
    
    public bool GetBossDefeated(int index) {
        if (index < bossesDefeated.Length) {
            return bossesDefeated[index];
        } else return false;
    }

}
