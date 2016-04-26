using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using Singleton;

public class FadingManager : Singleton<FadingManager>
{
    public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;
	/// <summary> Delai minimum en seconde </summary>
	public float min_delay = 3;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
	private int fadeDir = -1;

	private bool fading = false;
    private string scene;


    void OnGUI()
	{
		if (!this.fading)
			return;
		
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

	public void Fade(string sceneName = "menu")
	{
		StartCoroutine(this.LoadScene(sceneName));
	}

	private IEnumerator LoadScene(string sceneName)
	{
		this.fading = true;
		yield return StartCoroutine(this.FadeInAsync());
		yield return SceneManager.LoadSceneAsync("LoadingScreen");
		yield return StartCoroutine(this.FadeOutAsync());

		float endTime = Time.time + this.min_delay;

		var async = SceneManager.LoadSceneAsync(sceneName);
		async.allowSceneActivation = false;
		LoadingSceneManager.Async = async;
		while(!async.isDone)
		{
			if( async.progress >= 0.9f )
				break;
			yield return null;
		}

		if (Time.time < endTime) 
			yield return new WaitForSeconds (endTime - Time.time);

		LoadingSceneManager.Complete();
		yield return StartCoroutine(this.FadeInAsync());
		async.allowSceneActivation = true;

		LoadingSceneManager.UnloadLoadingScene();

		yield return StartCoroutine(this.FadeOutAsync());
		this.fading = false;
	}

	/// <summary> Display the fade </summary>
	public IEnumerator FadeInAsync()
	{
		this.alpha = 0f;
		this.fadeDir = 1;
		yield return new WaitForSeconds(this.fadeSpeed);
	}
	/// <summary> Remove the fade </summary>
	public IEnumerator FadeOutAsync()
	{
		this.alpha = 1f;
		this.fadeDir = -1;
		yield return new WaitForSeconds(this.fadeSpeed);
	}
}
