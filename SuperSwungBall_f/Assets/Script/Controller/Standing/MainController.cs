using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

using System.Net;
using System.Collections.Specialized;

namespace Standing
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private GameObject Press_To_Start;
        [SerializeField]
        private GameObject Authentication;

        private bool authenticate;
        private bool sync_ended;

		public MainController(){
		}

        // Use this for initialization
        void Awake()
        {
            //SaveLoad.save_user ();
			//SaveLoad.save_setting ();
			SaveLoad.load_settings();
			this.authenticate = SaveLoad.load_user();
			TranslateKit.Language.LoadLanguage (Settings.Instance.SelectedLanguage);

//			string gameId = "3";
//			string fileName = (Application.persistentDataPath + "/replay.txt");
//			string uri ="http://ssb.trendspotlight.fr/upload_replay.php";
//			NameValueCollection values = new NameValueCollection();
//			values.Add("winner", "antoine");
//			values.Add("looser", "ennemy");
//			values.Add("gameId", gameId);
//
//			HTTP.uploadFile (values, uri, fileName);

            if (this.authenticate)
            {
                HTTP.SyncUser((success) =>
                {
                    this.authenticate = success;
                    this.sync_ended = true;
                });
            }
        }

        // Update is called once per frame
        void Update()
        {
			if (Input.anyKeyDown && !Input.GetKey(KeyCode.LeftControl))
            {

                if (!this.authenticate)
                {
                    Press_To_Start.SetActive(false);
                    Authentication.SetActive(true);
                    return;
                }

                if (this.sync_ended)
					FadingManager.Instance.Fade();
            }
        }

    }
}

