using UnityEngine;
using System.Collections;

using Singleton;

public class DebugManager : Singleton<DebugManager>
{
    /// <summary>
    /// Test de class static
    /// Mettre tout les Inputs ici pour avoir une idée global des inputs disponibles et des actions de chacuns
    /// Pour activer un input : Ctrl Gauche + ...
    /// </summary>

#if DEBUG
    void Update()
    {

        if (!Input.GetKey(KeyCode.LeftControl))
            return;

        // -- Exemple
        //		if (Input.GetKeyDown (KeyCode.B))
        //			Debug.Log ("Ctrl + B detected");

        // -- SaveLoadUpdate
        if (Input.GetKeyDown(KeyCode.U))
        {
            bool suc = false;
            SaveLoad.reset_user();
            SaveLoad.reset_setting();
            HTTP.Authenticate("antoine", "mdp", (success) => { suc = success; });
            SaveLoad.save_user();
            SaveLoad.save_setting();
            if (suc)
            {
                Debug.Log("Setting Updated -- Connected with antoine");
            }
            else
            {
                Debug.LogError("Error Update Setting");
            }
        }

        // -- NotificationSend
        if (Input.GetKeyDown(KeyCode.J))
            Notification.Create(NotificationType.Slide, "My Title Slide", null);
        if (Input.GetKeyDown(KeyCode.K))
            Notification.Create(NotificationType.Box, "My Title Box", content: "My Content Box");
        if (Input.GetKeyDown(KeyCode.L))
            Notification.Alert("My Title Alert", "My Content Slide", completion: (success) =>
            {
                Debug.Log(success);
            });

        // -- MusicManager
        if (Input.GetKeyDown(KeyCode.C))
			MusicManager.Instance.Stop_Music();
        if (Input.GetKeyDown(KeyCode.F))
			MusicManager.Instance.Clip = "Musics/Team/PSG/Allez Paris [classic]";

        // -- NotificationState
        if (Input.GetKeyDown(KeyCode.W))
            Settings.Instance.NotificationState = NotificationState.All;
        if (Input.GetKeyDown(KeyCode.X))
            Settings.Instance.NotificationState = NotificationState.Private;
        if (Input.GetKeyDown(KeyCode.C))
            Settings.Instance.NotificationState = NotificationState.Nothing;


        if (Input.GetKeyDown(KeyCode.I))
        {
            User u = User.Instance;
			Debug.Log("UserInfo : username:" + u.username + " - id:" + u.id + " - phi:" + u.phi);
			Settings s = Settings.Instance;
			Debug.Log("TeamInfo : default:" + s.Default_Team.ToStringFull());
        }


		if (Input.GetKeyDown (KeyCode.T))
			StartCoroutine (FadingManager.Instance.FadeInAsync ());
		if (Input.GetKeyDown (KeyCode.O))
			StartCoroutine (FadingManager.Instance.FadeOutAsync ());

		if (Input.GetKeyDown (KeyCode.S))
			AudioListener.volume = 0f;
		if (Input.GetKeyDown (KeyCode.M))
			AudioListener.volume = 1f;
    }
#endif
}
