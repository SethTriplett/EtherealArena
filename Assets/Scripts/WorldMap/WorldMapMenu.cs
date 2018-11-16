using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMenu : MonoBehaviour {

	private bool menuActive = false;
	private CanvasGroup menuGroup;

    void Awake() {
        menuGroup = GetComponent<CanvasGroup>();
    }

    void Start() {
        AudioManager.GetInstance().StopMusic(Soundtrack.TutorialTheme);
        AudioManager.GetInstance().StopMusic(Soundtrack.VampireTheme);
        AudioManager.GetInstance().StopMusic(Soundtrack.PsychicTheme);
        AudioManager.GetInstance().StopMusic(Soundtrack.FinalBossTheme);
        AudioManager.GetInstance().StopMusic(Soundtrack.EndingTheme);

        AudioManager.GetInstance().StartMusic(Soundtrack.TitleTheme);
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
			} else {
				Time.timeScale = 1;
				menuActive = false;
                menuGroup.alpha = 0f;
                menuGroup.blocksRaycasts = false;
                menuGroup.interactable = false;
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
