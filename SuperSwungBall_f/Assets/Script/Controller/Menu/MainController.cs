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

		// Use this for initialization
		void Start () {
			if (User.Instance.is_connected) {
				account.SetActive (true);
				account_username.text = User.Instance.username;
				account_score.text = "Score : " + User.Instance.score;
			}
		}

		// Update is called once per frame
		void Update () {
		
		}

		public void deconnect(){
			User.Instance = new User ();
			SaveLoad.reset_user ();
			account.SetActive (false);
		}
	}
}
