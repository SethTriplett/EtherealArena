using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour {

    public RectTransform title;
    public CanvasGroup credits;

    public CanvasGroup musicCredits;
    public bool creditsOn;
    private float creditsAlpha;
    private bool musicCreditsOn;
    private float musicCreditsAlpha;

    private void Start() {
        AudioManager.GetInstance().StopMusic(Soundtrack.TutorialTheme);
        AudioManager.GetInstance().StopMusic(Soundtrack.VampireTheme);
        AudioManager.GetInstance().StopMusic(Soundtrack.PsychicTheme);
        AudioManager.GetInstance().StopMusic(Soundtrack.FinalBossTheme);
        AudioManager.GetInstance().StopMusic(Soundtrack.EndingTheme);

        AudioManager.GetInstance().StartMusic(Soundtrack.TitleTheme);
    }

    private void Update() {

        // Make title float

        float y = 60 + Mathf.Sin(Time.time * 2) * 4;
        float z = Mathf.Sin(Time.time);
        title.anchoredPosition = new Vector2(0, y);
        title.localRotation = Quaternion.Euler(0, 0, z);

        // Update credits

        creditsAlpha = Mathf.Clamp01(creditsAlpha + (creditsOn ? 5f : -5f) * Time.deltaTime);

        credits.interactable = creditsOn;
        credits.blocksRaycasts = creditsOn;
        credits.alpha = Mathf.SmoothStep(0, 1, creditsAlpha);
        
        musicCreditsAlpha = Mathf.Clamp01(musicCreditsAlpha + (musicCreditsOn ? 5f : -5f) * Time.deltaTime);

        musicCredits.interactable = musicCreditsOn;
        musicCredits.blocksRaycasts = musicCreditsOn;
        musicCredits.alpha = Mathf.SmoothStep(0, 1, musicCreditsAlpha);
    }

    public void PlayButton() {
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
        Vignette.LoadScene("WorldMap");
    }

    public void CreditsButton() {
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
        creditsOn = true;
    }
    
    public void MusicCreditsButton() {
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
        musicCreditsOn = true;
    }

    public void CreditsBackButton() {
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
        creditsOn = false;
    }
    
    public void MusicCreditsBackButton() {
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
        musicCreditsOn = false;
    }

    public void QuitButton() {
        AudioManager.GetInstance().PlaySound(Sound.MenuClick);
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}