using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour {

    private Bounds bounds = new Bounds(new Vector3(0f, 0f, 0f), new Vector3(30f, 20f, 20f));

    public bool IsOutOfBounds(Transform transform) {
        return !(bounds.Contains(transform.position));
    }

}