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
			//SaveLoad.save_user ();
			//SaveLoad.save_setting ();
			SaveLoad.load_settings ();
			SaveLoad.load_user ();
        }

        // Update is called once per frame
        void Update()
        {
			if (Input.GetKey(KeyCode.U)) {
				bool suc = false;
				HTTP.Authenticate ("antoine", "mdp", (success) => { suc = success; });
				SaveLoad.save_user ();
				SaveLoad.save_setting ();
				if (suc){
					Debug.Log("Setting Updated");
				}else {
					Debug.Log("Error Update Setting");
				}
			}
			if (Input.anyKey)
				FadingManager.I.Fade ();
        }
    }
}

