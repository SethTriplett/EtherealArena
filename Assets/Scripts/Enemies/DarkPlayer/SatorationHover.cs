using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatorationHover : MonoBehaviour {

    private const float amplitude = 0.2f;
    private const float period = 2f;

    void Update() {
        float currentFloat = Mathf.Sin(Time.time * 2 * Mathf.PI / period) * amplitude;
        float previousFloat = Mathf.Sin((Time.time - Time.deltaTime) * 2 * Mathf.PI / period) * amplitude;
        Vector3 deltaPos = new Vector3(0, currentFloat - previousFloat, 0);
        transform.position += deltaPos;
    }

}
