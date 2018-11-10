using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBackground : MonoBehaviour {
    
    private float angle;
    private float angularSpeed;

    void Awake() {
        angle = 90f;
        angularSpeed = 20f;
    }

    void Update() {
        angle += angularSpeed * Time.deltaTime;
        angle %= 360f;
        if (angle < 0f) angle += 360f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
