using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Vignette : MonoBehaviour {
	
	public static bool IsBusy { get { return instance.busy; } }

	private static Vignette instance;

	private bool busy = false;
	private Material mat = null;
    [SerializeField] private bool startActive; // Have the vignette active on the start screen

	private void Awake() {

		if (instance) {
			Destroy(this);
		} else {
			instance = this;
			DontDestroyOnLoad(this);
			
			Image i = GetComponentInChildren<Image>();
			mat = Instantiate(i.material);
            if (startActive) mat.SetFloat("_Alpha", 0.4f);
			i.material = mat;
		}
	}

	private void Update()
	{
		mat.SetFloat("_GlobalTime", Time.time);
	}

	private void OnDestroy()
	{
		Destroy(mat);
	}

	public static void LoadScene(string sceneName) {

		if (!IsBusy)
			instance.StartCoroutine(instance.LoadSceneRoutine(sceneName));
	}

	private IEnumerator LoadSceneRoutine(string sceneName) {

		busy = true;

		float startAlpha = mat.GetFloat("_Alpha");
		float fadeLength = 0.6f;
		float waitLength = 0.15f;

		// Close the vignette

		for (float f = 0; f < fadeLength; f += Time.deltaTime) {
			float t = Mathf.SmoothStep(startAlpha, 1, f / fadeLength);
			mat.SetFloat("_Alpha", t);
			yield return null;
		}

		mat.SetFloat("_Alpha", 1);

		// Wait a bit

		yield return new WaitForSeconds(waitLength);

		// Load the new scene

		AsyncOperation sceneLoad = SceneManager.LoadSceneAsync(sceneName);

		while (!sceneLoad.isDone)
			yield return null;

		// Wait a bit

		yield return new WaitForSeconds(waitLength);
		
		// Open the vignette

		for (float f = 0; f < fadeLength; f += Time.deltaTime) {
			float t = Mathf.SmoothStep(1, 0, f / fadeLength);
			mat.SetFloat("_Alpha", t);
			yield return null;
		}

		mat.SetFloat("_Alpha", 0);

		busy = false;
	}
}