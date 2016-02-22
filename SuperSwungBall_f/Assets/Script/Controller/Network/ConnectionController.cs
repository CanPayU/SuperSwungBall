using UnityEngine;
using UnityEngine.UI;
using System.Collections;


namespace Network {
	public class ConnectionController : MonoBehaviour {

		[SerializeField]
		private InputField username;
		[SerializeField]
		private InputField password;
		[SerializeField]
		private Button connect;

		// Use this for initialization
		void Start () {
		
		}
		
		public void check_login(){
			connect.enabled = false;
			HttpController controller = gameObject.GetComponent<HttpController>();
			controller.connect(username.text, password.text, (success) => {
				if(success) {
					//this.connection_panel.SetActive(false);
					//this.loading_panel.SetActive(true);
					Debug.Log("Connected");
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
			
		private void connection(){
			GameObject.Find ("Main").GetComponent<MainController> ().connect_network ();
		}
	}
}
