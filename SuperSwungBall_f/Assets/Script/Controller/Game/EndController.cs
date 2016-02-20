using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace GameScene
{
	public class EndController : MonoBehaviour {


		[SerializeField]
		private Text status;
		[SerializeField]
		private Text points;
		[SerializeField]
		private Text content;
		[SerializeField]
		private GameObject panel;


		private string status_text = "";
		private string points_text = "points : ";
		private string content_text = "";
		private Color status_color;

		private PhotonPlayer local_player;
		private PhotonPlayer other_player;

		// Use this for initialization
		void Start () {
			panel.SetActive (false);
			other_player = PhotonNetwork.otherPlayers [0];
			local_player = PhotonNetwork.player;
		}
		
		public void on_end (End type, object value = null) {

			switch (type) {
			case End.ABANDON:
				on_abandon (value as PhotonPlayer);
				break;
			case End.TIME:
				on_time ();
				break;
			}

			status.text = status_text;
			points.text = points_text;
			content.text = content_text;

			status.color = status_color;
			panel.SetActive (true);
		}

		private void on_abandon(PhotonPlayer player) {
			
			status_text = "Victoire";
			content_text = local_player.name + "\n VS \n" + player.name + " - Abandon";
			points_text += "calculate";
		}

		private void on_time() {
			int myScore = Game.Instance.Teams [local_player.ID].Points;
			int otherScore = Game.Instance.Teams [other_player.ID].Points;
			if (myScore > otherScore) {
				status_text = "Victoire";
				status_color = new Color (92f / 255f, 184f / 255f, 92f / 255f);
			} else {
				status_text = "Defaite";
				status_color = new Color (212f / 255f, 85f / 255f, 83f / 255f);
			}

			content_text = local_player.name + "\n VS \n" + other_player.name;
			points_text += "calculate";
		}
	}
}