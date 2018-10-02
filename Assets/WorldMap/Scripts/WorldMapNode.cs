
using System.Linq;
using UnityEngine;

namespace EtherealArena.WorldMap {

	public class WorldMapNode : MonoBehaviour {
		
        public Vector2 Position { get { return transform.position; } }

        private WorldMapEdge[] edges;
        
        // TODO: replace this property / field with the information about the battle that this node represents
        // which background it uses, what boss is inside it, etc.

        public EnemyType EnemyType { get { return enemyType; }}
        [SerializeField] private EnemyType enemyType;
        public int EnemyLevel { get { return enemyLevel; }}
        [SerializeField] private int enemyLevel;

        private void Start() {

            // When a node starts, it stores all the edges that are connected to it.

            edges = FindObjectsOfType<WorldMapEdge>().Where(e => e.IsConnectedTo(this)).ToArray();
        }

        public WorldMapNode GetNodeInDirection(Vector2 direction) {

            // Using the stored edge list, go through all the connected nodes and find the one that is
            // the most in the given direction. The initial value of bestAngle defines the cutoff for
            // any node being marked as good (basically, how precise you have to be when aiming.)

            WorldMapNode bestNode = null;
            float bestAngle = 100;

            foreach (WorldMapEdge edge in edges) {
                WorldMapNode node = edge.AcrossFrom(this);
                Vector2 delta = node.Position - Position;
                float angle = Vector2.Angle(direction, delta);

                if (angle < bestAngle) {
                    bestAngle = angle;
                    bestNode = node;
                }
            }

            return bestNode;
        }
	}

}