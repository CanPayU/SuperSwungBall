using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Standing
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private string scene;

		// a supp
		[SerializeField]
		private GameObject music;

        // Use this for initialization
        void Start()
        {
			SaveLoad.save_setting ();
			SaveLoad.load_settings ();
			SaveLoad.load_user ();
			Debug.Log (Settings.Instance.Default_Team);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.anyKey)
	            FadingManager.I.Fade ();
        }
    }
}

