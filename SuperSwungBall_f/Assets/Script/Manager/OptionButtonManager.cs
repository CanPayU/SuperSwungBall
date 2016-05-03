using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using Singleton;

public class OptionButtonManager : Singleton<OptionButtonManager> {

	private GameObject settings;
	private GameObject canvas;

	void Start()
	{
		this.canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		this.settings = Resources.Load("Prefabs/OptionButton/Settings") as GameObject;
	}

	void OnLevelWasLoaded(int level) {
		this.canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		Debug.Log ("Loaded : " + level);
		if (level < 2)
			return;
		GameObject settingsGm = Instantiate(settings);
		settingsGm.name = "Settings";
		settingsGm.transform.SetParent(canvas.transform, false);
		Debug.Log ("Settings Instanciated");
	}
}
