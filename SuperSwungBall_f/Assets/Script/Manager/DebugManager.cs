using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour {


	/// <summary>
	/// Test de class static
	/// Mettre tout les Inputs ici pour avoir une idée global des inputs disponibles et des actions de chacuns
	/// Pour activer un input : Ctrl Gauche + ...
	/// </summary>

	#if DEBUG
	void Update () {

		if (!Input.GetKey (KeyCode.LeftControl))
			return;

		// -- Exemple
//		if (Input.GetKeyDown (KeyCode.B))
//			Debug.Log ("Ctrl + B detected");

		// -- SaveLoadUpdate
		if (Input.GetKeyDown(KeyCode.U)) {
			bool suc = false;
			SaveLoad.reset_user ();
			SaveLoad.reset_setting ();
			HTTP.Authenticate ("antoine", "mdp", (success) => { suc = success; });
			SaveLoad.save_user ();
			SaveLoad.save_setting ();
			if (suc){
				Debug.Log("Setting Updated -- Connected with antoine");
			}else {
				Debug.LogError("Error Update Setting");
			}
		}

		// -- NotificationSend
		if(Input.GetKeyDown(KeyCode.J))
			Notification.Create(NotificationType.Slide, "My Title Slide");
		if(Input.GetKeyDown(KeyCode.K))
			Notification.Create(NotificationType.Box, "My Title Box", content: "My Content Box");
		if(Input.GetKeyDown(KeyCode.L))
			Notification.Create(NotificationType.Alert, "My Title Alert", content: "My Content Slide", completion: (success) => {
				Debug.Log(success);
			});

		// -- MusicManager
		if (Input.GetKeyDown(KeyCode.C))
			MusicManager.I.Stop_Music();
		if (Input.GetKeyDown(KeyCode.F))
			MusicManager.I.Clip = "Musics/Team/PSG/Allez Paris [classic]";

		// -- NotificationState
		if (Input.GetKeyDown (KeyCode.W))
			Settings.Instance.NotificationState = NotificationState.All;
		if (Input.GetKeyDown (KeyCode.X))
			Settings.Instance.NotificationState = NotificationState.Private;
		if (Input.GetKeyDown (KeyCode.C))
			Settings.Instance.NotificationState = NotificationState.Nothing;


		
	}
	#endif
}
