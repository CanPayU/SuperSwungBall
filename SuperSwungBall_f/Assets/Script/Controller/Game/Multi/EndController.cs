﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace GameScene.Multi
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
		[SerializeField]
		private GameObject btn_quit;
		[SerializeField]
		private string scene;

		private string status_text = "";
		private string points_text = "points : ";
		private string content_text = "";
		private Color status_color;
		private bool win = false;
		private int score = 0;

		private PhotonPlayer local_player;
		private PhotonPlayer other_player;

		// Use this for initialization
		void Start () {
			panel.SetActive (false);
			other_player = PhotonNetwork.otherPlayers [0];
			local_player = PhotonNetwork.player;
		}
		
		public void on_end (End type, object value = null) {

			Game.Instance.isFinish = true;
			switch (type) {
			case End.ABANDON:
				on_abandon (value as PhotonPlayer);
				break;
			case End.TIME:
				on_time ();
				break;
			}


			if (PhotonNetwork.room.visible) {
				calculate_point ();
				points_text += "" + score;
			} else {
				points_text = "Partie privee";
			}

			status.text = status_text;
			points.text = points_text;
			content.text = content_text;

			status.color = status_color;
			panel.SetActive (true);

			StartCoroutine(Diconnect());
		}

		void calculate_point(){
			if (!win) {
				score = 0;
			} else {
				score = 10;
			}

			HttpController controller = gameObject.GetComponent<HttpController>();
			controller.sync_score(score, (success) => {
				if(!success)
					Notification.danger("Erreur de synchronisation");
				btn_quit.SetActive(true);
			});
		}

		IEnumerator Diconnect()
		{
			yield return new WaitForSeconds(5);
			PhotonNetwork.LeaveRoom ();
			PhotonNetwork.Disconnect ();
		}

		void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			if (!Game.Instance.isFinish) {
				on_end(End.ABANDON, otherPlayer);
			}
		}

		private void on_abandon(PhotonPlayer player) {
			win = true;
			status_text = "Victoire";
			status_color = new Color (92f / 255f, 184f / 255f, 92f / 255f);
			content_text = local_player.name + "\n VS \n" + player.name + " - Abandon";
		}

		private void on_time() {
			int myScore = Game.Instance.Teams [local_player.ID].Points;
			int otherScore = Game.Instance.Teams [other_player.ID].Points;
			if (myScore > otherScore) {
				win = true;
				status_text = "Victoire";
				status_color = new Color (92f / 255f, 184f / 255f, 92f / 255f);
			} else {
				status_text = "Defaite";
				status_color = new Color (212f / 255f, 85f / 255f, 83f / 255f);
			}

			content_text = local_player.name + "\n VS \n" + other_player.name;
		}
		public void exit()
		{
			FadingManager.I.Fade (scene);
			//StartCoroutine(ChangeLevel());

			PhotonNetwork.LeaveRoom ();
			PhotonNetwork.Disconnect ();
		}
		/*
		IEnumerator ChangeLevel()
		{
			float fadeTime = GameObject.Find("GM_Fade").GetComponent<Fading>().BeginFade(1);
			yield return new WaitForSeconds(fadeTime);
			SceneManager.LoadScene(scene);
		}
		*/
	}
}