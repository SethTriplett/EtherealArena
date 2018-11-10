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
    public void SetAngle(float angle) {
        setWangle(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f));
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
    }

    void OnEnable (){
        EventMessanger.instance.SubscribeEvent(typeof(DeleteAttacksEvent),this);
    }
    void OnDisable (){
        EventMessanger.instance.UnsubscribeEvent(typeof(DeleteAttacksEvent),this);
    }
        
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            if (other.gameObject.layer != 8) {
                PlayerStatus playerStatus = other.gameObject.GetComponent<PlayerStatus>();
                if (playerStatus != null) {
                    playerStatus.TakeHit();
                } else {
                    Debug.LogError("No player status script found.");
                }
                speed = 0f;
                gameObject.SetActive(false);
            }
        }
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
