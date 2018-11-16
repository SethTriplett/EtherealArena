using UnityEngine;

namespace EtherealArena.WorldMap {

	public class WorldMapEdge : MonoBehaviour {
		
		[SerializeField] private WorldMapNode a;
        [SerializeField] private WorldMapNode b;
        private bool canCross;

        // Updates the line positions when the references to A or B change.
        // Also runs when the scene starts.

        private void OnValidate() {
            LineRenderer line = GetComponent<LineRenderer>();

            if (a != null && b != null && a != b) {
                line.enabled = true;

                for (int i = 0; i <= 4; i++) {
                    float t = i / 4f;
                    Vector3 v = Vector3.Lerp(a.Position, b.Position, t);
                    line.SetPosition(i, v);
                }

            } else {
                line.enabled = false;
            }
        }

        // Given a node, returns the other connected node.
        // Returns null if the given node is not A or B.

        public WorldMapNode AcrossFrom(WorldMapNode node) {
            if (node == a)
                return b;
            if (node == b)
                return a;
            return null;
        }

        // Returns if this edge connects to the given node.

        public bool IsConnectedTo(WorldMapNode node) {
            return (node == a || node == b);
        }
        
        public bool GetCanCross() {
            return canCross;
        }

        public void SetCanCross(bool canCross) {
            this.canCross = canCross;
        }

	}

}