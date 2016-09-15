using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using Singleton;

public class OptionButtonManager : Singleton<OptionButtonManager>
{

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

    private GameObject account;
    private GameObject settings;
    private GameObject home;

    void Start()
    {
        this.canvas = FindObjectOfType<Canvas>().gameObject;
        this.account = Resources.Load("Prefabs/OptionButton/Account") as GameObject;
        this.settings = Resources.Load("Prefabs/OptionButton/Settings") as GameObject;
        this.home = Resources.Load("Prefabs/OptionButton/Home") as GameObject;

        // Correction OnLevelWasLoaded
        SceneManager.sceneLoaded += (scene, loadingMode) =>
        {
            //pas de boutons dans ces scenes :
            if (scene.name == "standing" || scene.name == "LoadingScreen")
                return;

            this.canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
            instanciateBtn(this.settings, "Settings");

            if (User.Instance.is_connected)
                instanciateBtn(this.account, "Account");

            // bouton Home dans toutes les scenes sauf :
            if (scene.name != "menu")
                instanciateBtn(this.home, "Home");
        };
    }
    /*
	void OnLevelWasLoaded(int level) 
	{
		if (level < 2)
			return;
		
		this.canvas = GameObject.FindObjectOfType<Canvas>().gameObject;
		instanciateBtn (this.settings, "Settings");
		if (User.Instance.is_connected)
			instanciateBtn (this.account, "Account");

		if (level != 8)
			instanciateBtn (this.home, "Home");
	}
    */

    private void instanciateBtn(GameObject btn, string name)
    {
        GameObject gm = Instantiate(btn);
        gm.name = name;
        gm.transform.SetParent(canvas.transform, false);
    }
}
