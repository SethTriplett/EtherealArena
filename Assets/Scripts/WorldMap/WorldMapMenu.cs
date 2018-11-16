using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EtherealArena.WorldMap {

    public class WorldMapMenu : MonoBehaviour {

        private bool menuActive = false;
        private CanvasGroup menuGroup;

        void Awake() {
            menuGroup = GetComponent<CanvasGroup>();
        }

        void Update() {
            if (Input.GetButtonDown("Start")) {
                AudioManager.GetInstance().PlaySound(Sound.MenuClick);
                if (menuActive == false) {
                    Time.timeScale = 0;
                    menuActive = true;
                    menuGroup.alpha = 1f;
                    menuGroup.blocksRaycasts = true;
                    menuGroup.interactable = true;
                    EventMessanger.GetInstance().TriggerEvent(new WorldMapMenuEvent(true));
                } else {
                    Time.timeScale = 1;
                    menuActive = false;
                    menuGroup.alpha = 0f;
                    menuGroup.blocksRaycasts = false;
                    menuGroup.interactable = false;
                    EventMessanger.GetInstance().TriggerEvent(new WorldMapMenuEvent(false));
                }
            }
        }
        
        public void ResumeButton() {
            AudioManager.GetInstance().PlaySound(Sound.MenuClick);
            Time.timeScale = 1;
            menuActive = false;
            menuGroup.alpha = 0f;
            menuGroup.blocksRaycasts = false;
            menuGroup.interactable = false;
            EventMessanger.GetInstance().TriggerEvent(new WorldMapMenuEvent(false));
        }

        public void QuitToMenu(){
            AudioManager.GetInstance().PlaySound(Sound.MenuClick);
            Time.timeScale = 1;
            Vignette.LoadScene("TitleScreen");
            menuActive = false;
            menuGroup.alpha = 0f;
            menuGroup.blocksRaycasts = false;
            menuGroup.interactable = false;
        }

    }

}
