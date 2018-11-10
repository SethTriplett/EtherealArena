using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	public bool gamePaused = false;
	public GameObject pauseMenu;


	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Cancel"))
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
		Time.timeScale = 1;
		gamePaused = false;
		pauseMenu.SetActive(false);
	}

	public void QuitToMenu(){
		Time.timeScale = 1;
		Vignette.LoadScene("TitleScreen");
		pauseMenu.SetActive(false);
	}

	public void QuitToWorld(){
		Time.timeScale = 1;
		Vignette.LoadScene("WorldMap");
		pauseMenu.SetActive(false);
	}
}
