using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using Singleton;

public class OptionButtonManager : Singleton<OptionButtonManager> {

	/// LevelIndex
	/// 0 Standing
	/// 1 Loading
	/// 2 Didact
	/// 3 SoloGame
	/// 4 game
	/// 5 network
	/// 6 create_team
	/// 7 gestion
	/// 8 menu

	private GameObject canvas;

	private GameObject settings;
	private GameObject home;

	void Start()
	{
		this.canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		this.settings = Resources.Load("Prefabs/OptionButton/Settings") as GameObject;
		this.home = Resources.Load("Prefabs/OptionButton/Home") as GameObject;
	}

	void OnLevelWasLoaded(int level) 
	{
		if (level < 2)
			return;
		
		this.canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		instanciateSettingsBtn ();

		if (level != 8)
			instanciateHomeBtn ();
	}

	private void instanciateSettingsBtn()
	{
		GameObject gm = Instantiate(settings);
		gm.name = "Settings";
		gm.transform.SetParent(canvas.transform, false);
	}

	private void instanciateHomeBtn()
	{
		GameObject gm = Instantiate(home);
		gm.name = "Home";
		gm.transform.SetParent(canvas.transform, false);
	}
}
