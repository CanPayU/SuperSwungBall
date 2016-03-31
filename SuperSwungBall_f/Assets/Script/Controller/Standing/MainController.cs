using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Standing
{
    public class MainController : MonoBehaviour
    {
		[SerializeField] private GameObject Press_To_Start;
		[SerializeField] private GameObject Authentication;

		private bool authenticate;

        // Use this for initialization
        void Start()
        {
			//SaveLoad.save_user ();
			//SaveLoad.save_setting ();
			SaveLoad.load_settings ();
			this.authenticate = SaveLoad.load_user ();
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
			if (Input.anyKey) {
				if (authenticate)
					FadingManager.I.Fade ();
				else {
					Press_To_Start.SetActive (false);
					Authentication.SetActive (true);
				}
			}
        }
    }
}

