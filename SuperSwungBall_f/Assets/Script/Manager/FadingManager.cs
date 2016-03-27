using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadingManager : MonoBehaviour {

	private static FadingManager instance = null;
	/// <summary>
	/// Instance static de la class
	/// </summary>
	public static FadingManager I
	{
		get { return instance; }
	}

	void Awake()
	{
		if ((instance != null && instance != this))
		{	Destroy(this.gameObject);	return;	}
		else
			instance = this;
		DontDestroyOnLoad(this.gameObject);
	}

	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;

	private int drawDepth = -1000;
	private float alpha = 1.0f;
	private int fadeDir = -1;

	private string scene;

	void OnGUI()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		GUI.color = new Color(GUI.color.r,GUI.color.g,GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	private float BeginFade(int direction)
	{
		fadeDir = direction;
		return fadeSpeed;
	}

	void OnLevelWasLoaded(int level)
	{
		StartCoroutine(ChangeLevel());
	}

	IEnumerator ChangeLevel()
	{
		float fadeTime = this.BeginFade(-1);
		yield return new WaitForSeconds(fadeTime);
	}

	public void Fade(string scene_ = "menu")
	{
		I.scene = scene_;
		FadingManager.I.Start_Fade ();
	}

	private void Start_Fade(){
		StartCoroutine(ChangeScene());
	}

	IEnumerator ChangeScene()
	{
		float fadeTime = BeginFade(1);
		yield return new WaitForSeconds(fadeTime);
		SceneManager.LoadScene(scene);
	}
}
