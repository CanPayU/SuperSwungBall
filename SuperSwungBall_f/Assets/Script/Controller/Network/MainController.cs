using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Network {
	public class MainController : MonoBehaviour {

		[SerializeField]
		private InputField username;
		[SerializeField]
		private InputField password;
		[SerializeField]
		private Button connect;
		[SerializeField]
		private GameObject connection_panel;
		[SerializeField]
		private GameObject loading_panel;

		// Use this for initialization
		void Start () {
			if (!User.Instance.is_connected)
				loading_panel.SetActive (false);
			else
				connection_panel.SetActive (false);
			/*
			 // sync Score with WebSite
				int score = 15;
				Debug.Log(User.Instance.score);
				HttpController controller = gameObject.GetComponent<HttpController>();
				controller.sync_score(score, (success) => {
					Debug.Log(success);
					Debug.Log(User.Instance.score);
				});


				// localhost
				string username = "antoine"; // id = 1
				string password = "mdp"; // OK
			*/
				

		}

		public void check_login(){
			connect.enabled = false;
			HttpController controller = gameObject.GetComponent<HttpController>();
			controller.connect(username.text, password.text, (success) => {
				Debug.Log("isConnected ? " + User.Instance.is_connected + " - Success ?" + success);
				if(success) {
					this.connection_panel.SetActive(false);
					this.loading_panel.SetActive(true);
					SaveLoad.save_user();
					this.connection();
				}else {
					ColorBlock color = connect.colors;
					color.normalColor = new Color (212f / 255f, 85f / 255f, 83f / 255f);
					connect.image.color = new Color (212f / 255f, 85f / 255f, 83f / 255f);
					connect.enabled = true;
				}
			});
		}

		public void connection(){
			
			GameObject.Find ("Network").GetComponent<NetworkController>().connect();
		}
	}
}
