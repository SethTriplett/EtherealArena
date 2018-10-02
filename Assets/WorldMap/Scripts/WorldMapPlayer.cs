using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EtherealArena.WorldMap {

	public class WorldMapPlayer : MonoBehaviour {

		[SerializeField] private WorldMapNode gameStartNode;
		
		[Space(10)]
		[Header("Settings & References")]
		[SerializeField] private float movementSpeed;
		[SerializeField] private Transform spriteTransform;

		// ========================================================================================

		private static WorldMapNode CURRENT_NODE;
		private bool canMove = true;

		private void Awake() {

			// If the current node is null, it's the first time the world map has loaded.
			// Set the current node to the game start node.

			CURRENT_NODE = gameStartNode;

			// Start the player at the current node.

			transform.position = CURRENT_NODE.transform.position;
		}

		private void Update() {

			// Let the player move from node to node or enter a level

			if (canMove) {

        		float h = Input.GetAxis("Horizontal");
        		float v = Input.GetAxis("Vertical");
				Vector2 input = new Vector2(h, v);

				if (input.magnitude > 0.5f) {

					WorldMapNode newNode = CURRENT_NODE.GetNodeInDirection(input);
					if (newNode != null)
						StartCoroutine(MoveToNewNode(newNode));

				} else if (Input.GetButtonDown("A")) {

					// TODO: replace this with an actual transition later.
					// the scene changing itself should probably be done in another class, after a nice fade out

                    LoadBattleSceneScript loader = GameControllerScript.GetInstance().GetComponent<LoadBattleSceneScript>();
                    loader.LoadBattleScene(CURRENT_NODE.EnemyType, CURRENT_NODE.EnemyLevel);

				}
			}

			// Make the player sprite hover slightly

			float hover = Mathf.Sin(Time.time * 8f) * 0.02f;
			spriteTransform.localPosition = new Vector3(0, 0.75f + hover, -0.1f);
		}

		private IEnumerator MoveToNewNode(WorldMapNode newNode) {
			
			canMove = false;

			// Make the player sprite face in the movement direction

			bool facingRight = newNode.Position.x > CURRENT_NODE.Position.x;
			spriteTransform.localScale = new Vector3(facingRight ? 1f : -1f, 1f, 1f) * 0.5f;

			// Play an animation of moving to the new node

			float distance = Vector3.Distance(CURRENT_NODE.Position, newNode.Position);
			float length = distance / movementSpeed;

			for (float f = 0; f < length; f += Time.deltaTime)
			{
				float t = Mathf.SmoothStep(0, 1, f / length);
				transform.position = Vector3.Lerp(CURRENT_NODE.Position, newNode.Position, t);

				yield return null;
			}

			// Set the current node to the new node and allow movement again

			transform.position = newNode.Position;
			CURRENT_NODE = newNode;

			canMove = true;
		}
	}
}