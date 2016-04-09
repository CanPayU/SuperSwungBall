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

			Debug.Log (User.Instance);
			Debug.Log (User.Instance.Friends);
        }

        // Update is called once per frame
        void Update()
        {
			if (Input.anyKey) {
				//return;
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

