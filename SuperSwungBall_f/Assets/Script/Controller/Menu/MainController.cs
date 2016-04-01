using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Menu
{
	public class MainController : MonoBehaviour {

		[SerializeField]
		private GameObject account;
		[SerializeField]
		private Text account_username;
		[SerializeField]
		private Text account_score;

		private Timer time;

		// Use this for initialization
		void Start () {
			if (User.Instance.is_connected) {
				account.SetActive (true);
				account_username.text = User.Instance.username;
				account_score.text = "Score : " + User.Instance.score;
			}
			time = new Timer (60.0F, Inactive);
			time.start ();
		}

		// Update is called once per frame
		void Update () {
			time.update ();
			if (Input.anyKey || Input.GetAxis("Mouse X") != 0 ||  Input.GetAxis("Mouse Y") != 0) {
				time.reset ();
				time.start ();
			}
        }

		void Inactive(){
			FadingManager.I.Fade ("standing");
		}

		public void deconnect(){
			User.Instance = new User ();
			SaveLoad.reset_user ();
			account.SetActive (false);
		}

		public void SaveParameters(){
			Debug.Log ("A FAIRE : SAVE SETTINGS");
		}
	}
}