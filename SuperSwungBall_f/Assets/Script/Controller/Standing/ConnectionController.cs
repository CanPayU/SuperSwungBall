using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Standing {
	
	public class ConnectionController : MonoBehaviour {

		[SerializeField] private InputField username;
		[SerializeField] private InputField password;
		[SerializeField] private Button connect;
		[SerializeField] private Sprite OnEchec;

		// Use this for initialization
		void Start () {

		}

		public void check_login(){
			connect.enabled = false;
			HTTP.Authenticate(username.text, password.text, (success) => {
				if(success) {
					Debug.Log("Connected");
					SaveLoad.save_user();
					FadingManager.I.Fade();
				}else {
					connect.image.sprite = OnEchec;
				}
			});
		}

		public void LinkInscription(){
			Application.OpenURL ("http://ssb.shost.ca/register/");
		}
	}
}

