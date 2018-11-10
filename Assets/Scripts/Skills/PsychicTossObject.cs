using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PsychicTossObject : MonoBehaviour , IEventListener {
    private GameObject owner;
    Vector3 wangle;
    float speed = 1f;

    public void setWangle(Vector3 angle){
        wangle = angle;    
        wangle *= speed;    
    }
    public void setSpeed (float sped){
        speed = sped;
    }
    public void setOwner (GameObject woner){
        owner = woner;
    }
    void Start(){
        wangle *= speed;
    }

    void Update() {
        Vector3 target = transform.position;
        target += wangle*Time.deltaTime;
        transform.position = target;
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation = new Vector3(rotation.x, rotation.y, rotation.z - 75 * Time.deltaTime);
        transform.rotation = Quaternion.Euler(rotation);
        if (transform.position.x > 11) {
            transform.position = new Vector3(-11f, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -11) {
            transform.position = new Vector3(11f, transform.position.y, transform.position.z);
        }
        if (transform.position.y > 8) {
            transform.position = new Vector3(transform.position.x ,-8f , transform.position.z);
        }
        if (transform.position.y < -8) {
            transform.position = new Vector3(transform.position.x ,+8f , transform.position.z);
        }
    }

    void OnEnable (){
        EventMessanger.instance.SubscribeEvent(typeof(DeleteAttacksEvent),this);
    }
    void OnDisable (){
        EventMessanger.instance.UnsubscribeEvent(typeof(DeleteAttacksEvent),this);
    }

    public void ConsumeEvent (IEvent e){
        if (e.GetType() == typeof(DeleteAttacksEvent)){
            DeleteAttacksEvent delly = e as DeleteAttacksEvent; 
            if (delly.owner == owner) {
                Deactivate();
            }
        }
    }
    private void Deactivate(){
        gameObject.SetActive(false);
    }
}
