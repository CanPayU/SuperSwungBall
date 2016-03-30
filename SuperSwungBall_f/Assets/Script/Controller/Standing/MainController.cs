using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Standing
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private string scene;

        // Use this for initialization
        void Start()
        {
			HTTP.Authenticate("antoine","mdp", (success) => {
				Debug.Log(success);
			});

			//SaveLoad.save_user ();
			//SaveLoad.save_setting ();
			SaveLoad.load_settings ();
			SaveLoad.load_user ();

			Debug.Log(Settings.Instance.version);
			Debug.Log(Settings.VERSION);
        }

        // Update is called once per frame
        void Update()
        {
			#if DEBUG
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
			#endif
			if (Input.anyKey)
				FadingManager.I.Fade ();
        }
    }
}

