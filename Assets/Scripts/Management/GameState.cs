using EtherealArena.WorldMap;

// All the variables we need to save
[System.Serializable]
public class GameState {

    private int currentNode;
    private bool[,] bossesDefeated;

    public GameState() {
        Initialize();
    }

    void Initialize() {
        currentNode = 0;
        bossesDefeated = new bool[4,4];
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {
                bossesDefeated[i, j] = false;
            }
        }
    }

    public void SetCurrentNode(int nodeIndex) {
        this.currentNode = nodeIndex;
    }
    
    public int GetCurrentNode() {
        return this.currentNode;
    }

    public void SetBossDefeated(EnemyType type, int phase) {
        int typeIndex = (int) type;
        if (typeIndex < bossesDefeated.GetLength(0) && phase < bossesDefeated.GetLength(1)) {
            bossesDefeated[typeIndex, phase] = true;
        }
    }
    
    public bool GetBossDefeated(EnemyType type, int phase) {
        int typeIndex = (int) type;
        if (typeIndex < bossesDefeated.GetLength(0) && phase < bossesDefeated.GetLength(1)) {
            return bossesDefeated[typeIndex, phase];
        } else return false;
    }

}
