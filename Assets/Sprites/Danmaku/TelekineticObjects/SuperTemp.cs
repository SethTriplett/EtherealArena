using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperTemp : MonoBehaviour {

    void Update() {
        Vector3 target = transform.position;
        target = new Vector3(target.x + 5 * Time.deltaTime, target.y, target.z);
        transform.position = target;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation = new Vector3(rotation.x, rotation.y, rotation.z - 75 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rotation);
        if (transform.position.x > 11) {
            transform.position = new Vector3(-11f, transform.position.y, transform.position.z);
        }
    }

}
