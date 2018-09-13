using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EphemeralArena.WorldMap {
	public class WorldMapCamera : MonoBehaviour {

		[SerializeField] private Transform follow;
		[SerializeField] private float followStrength;
		[SerializeField] private Vector3 offset;

		private void Start() {
			transform.position = follow.position + offset;
		}

		private void Update() {
			float dt = Time.deltaTime * 60f;
			transform.position = Vector3.Lerp(transform.position, follow.position + offset, followStrength * dt);
		}
	}
}