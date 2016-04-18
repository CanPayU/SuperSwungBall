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
		private bool sync_ended;

        // Use this for initialization
        void Start()
        {
			//SaveLoad.save_user ();
			//SaveLoad.save_setting ();
			SaveLoad.load_settings ();
			this.authenticate = SaveLoad.load_user ();

			if (this.authenticate) {
				HTTP.SyncUser ((success) => {
					Debug.Log("EndSync : " + success);
					this.authenticate = success;
					this.sync_ended = true;
				});
			}
        }

        // Update is called once per frame
        void Update()
        {
			if (Input.anyKey) {

				if(!this.authenticate){
					Press_To_Start.SetActive (false);
					Authentication.SetActive (true);
					return;
				}

				if(this.sync_ended)
					FadingManager.I.Fade ();
			}
        }
    }
}

