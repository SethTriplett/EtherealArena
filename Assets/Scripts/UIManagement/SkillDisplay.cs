using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDisplay : MonoBehaviour, IEventListener {

    private GameObject skill1;
    private GameObject skill2;
    private GameObject skill3;

    void Awake() {
        skill1 = transform.Find("Skill Icon (1)").gameObject;
        skill2 = transform.Find("Skill Icon (2)").gameObject;
        skill3 = transform.Find("Skill Icon (3)").gameObject;
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(SkillIconEvent), this);
    }
    
    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(SkillIconEvent), this);
    }

    private void SwitchIcon(int type) {
        skill1.SetActive(false);
        skill2.SetActive(false);
        skill3.SetActive(false);

        switch(type) {
            case 1:
                skill1.SetActive(true);
                break;
            case 2:
                skill2.SetActive(true);
                break;
            case 3:
                skill3.SetActive(true);
                break;
        }
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(SkillIconEvent)) {
            SkillIconEvent iconEvent = e as SkillIconEvent;
            SwitchIcon(iconEvent.type);
        }
    }
}
