using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	private bool gamePaused = false;
	public GameObject pauseMenu;


	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Start"))
		{
			if (gamePaused == false)
			{
				Time.timeScale = 0;
				gamePaused = true;
				pauseMenu.SetActive(true);
			}
			else
			{
				pauseMenu.SetActive(false);
				gamePaused = false;
				Time.timeScale = 1;
			}

		}
	}
	
	public void ResumeButton() {
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
		Time.timeScale = 1;
		gamePaused = false;
		pauseMenu.SetActive(false);
	}

    public void RestartButton() {
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);
    }

	public void QuitToMenu(){
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
		Time.timeScale = 1;
		Vignette.LoadScene("TitleScreen");
		pauseMenu.SetActive(false);
	}

	public void QuitToWorld(){
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
		Time.timeScale = 1;
		Vignette.LoadScene("WorldMap");
		pauseMenu.SetActive(false);
	}
}
