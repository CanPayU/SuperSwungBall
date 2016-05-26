using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace OptionButton {
	
	public class HomeController : MonoBehaviour 
	{
		private Button btn;

		// Use this for initialization
		void Start () 
		{
			this.btn = gameObject.GetComponent<Button> ();
			if (this.btn != null)
			{
				this.btn.onClick.AddListener(delegate ()
					{
						OnHome();
					});
			}
		}

		private void OnHome () 
		{
			if (PhotonNetwork.inRoom) {
				PhotonNetwork.LeaveRoom ();
				PhotonNetwork.Disconnect();
			}
			FadingManager.Instance.Fade();
		}
	}
}
