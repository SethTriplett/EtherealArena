using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundLoader : MonoBehaviour, IEventListener {

    private Image backgroundImage;
    [SerializeField] private Sprite vampireBackground;
    [SerializeField] private Sprite telepathicBackground;
    [SerializeField] private Sprite darkBackground;
    private GameObject darkPortalChild;

    void Awake() {
        backgroundImage = GetComponent<Image>();
        darkPortalChild = transform.Find("Dark Portal").gameObject;
    }

    void OnEnable() {
        EventMessanger.GetInstance().SubscribeEvent(typeof(BackgroundLoadEvent), this);
    }
    
    void OnDisable() {
        EventMessanger.GetInstance().UnsubscribeEvent(typeof(BackgroundLoadEvent), this);
    }

    private void LoadBackground(BackgroundEnum backgroundEnum) {
        switch(backgroundEnum) {
            case BackgroundEnum.VampireBackground:
                backgroundImage.sprite = vampireBackground;
                break;
            case BackgroundEnum.TelepathicBackground:
                backgroundImage.sprite = telepathicBackground;
                break;
            case BackgroundEnum.DarkPortal:
                backgroundImage.sprite = darkBackground;
                darkPortalChild.SetActive(true);
                break;
        }
    }

    public void ConsumeEvent(IEvent e) {
        if (e.GetType() == typeof(BackgroundLoadEvent)) {
            BackgroundLoadEvent backgroundLoadEvent = e as BackgroundLoadEvent;
            LoadBackground(backgroundLoadEvent.background);
        }
    }

}
