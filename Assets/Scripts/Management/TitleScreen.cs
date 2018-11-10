using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour {

    public RectTransform title;
    public CanvasGroup credits;

    public bool creditsOn;
    private float creditsAlpha;

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
    }

    public void PlayButton() {
        Vignette.LoadScene("WorldMap");
    }

    public void CreditsButton() {
        creditsOn = true;
    }

    public void CreditsBackButton() {
        creditsOn = false;
    }

    public void QuitButton() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}