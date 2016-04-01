using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour {


	/// <summary>
	/// Test de class static
	/// Mettre tout les Input ici pour avoir une idée global des inputs disponibles et des actions de chacuns
	/// </summary>



	// Use this for initialization
	void Start () {
	
	}
	
	#if DEBUG
	void Update () {

	
		if (Input.GetKey(KeyCode.U)) {
			bool suc = false;
			HTTP.Authenticate ("antoine", "mdp", (success) => { suc = success; });
			SaveLoad.save_user ();
			Settings.Instance = new Settings();
			SaveLoad.save_setting ();
			if (suc){
				Debug.Log("Setting Updated -- Connected with antoine");
			}else {
				Debug.LogError("Error Update Setting");
			}
		}

		if(Input.GetKey(KeyCode.J))
			Notification.Create(NotificationType.Slide, "My Title Slide");
		if(Input.GetKey(KeyCode.K))
			Notification.Create(NotificationType.Box, "My Title Box", content: "My Content Box");
		if(Input.GetKey(KeyCode.L))
			Notification.Create(NotificationType.Alert, "My Title Alert", content: "My Content Slide", completion: (success) => {
				Debug.Log(success);
			});

		if (Input.GetKeyDown(KeyCode.C))
			MusicManager.I.Stop_Music();
		if (Input.GetKeyDown(KeyCode.F))
			MusicManager.I.Clip = "Musics/Team/PSG/Allez Paris [classic]";
	}
	#endif
}
