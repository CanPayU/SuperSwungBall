using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

using GameScene;

namespace GameScene.Multi
{
	public class MainController : BasicMainController
	{
		[SerializeField]
		private GameObject ball_prefab;

		private PhotonPlayer other_player;

		// Use this for initialization
		protected override void Start()
		{
			this.playerController = typeof(GameScene.Multi.PlayerController);
			this.ball_prefab = Resources.Load("Prefabs/Resources/Ball") as GameObject;
			this.other_player = PhotonNetwork.otherPlayers[0];
			Team ennemy_t = (Team)other_player.allProperties["Team"];
			Game.Instance = new Game(ennemy_t);
			base.Start ();
		}


		// Update is called once per frame
		protected override void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
			{
				// Sync StartAnnim
				PhotonView pv = PhotonView.Get(this);
				pv.RPC("start_annim", PhotonTargets.All);
			}
			time.update();
			updateTimerText ();
		}

		private void updateTimerText(){
			if (this.annim_started) {
				this.timerText.text = string.Empty;
				return;
			}
			this.timerText.text = (this.time.Time_remaining).ToString ("0");
			if (this.time.Time_remaining < 10f)
				this.timerAnimator.Play ("EndTimer");
			else
				this.timerAnimator.Play ("Empty");
		}

		[PunRPC]
		protected override void end_time()
		{
			// if  is MasterClient
			this.time.reset();
			if (annim_started)
				Caller.StartReflexion();
			else
			{ 
				// Sync StartAnnim
				PhotonView pv = PhotonView.Get(this);
				pv.RPC("start_annim", PhotonTargets.All);
			}
		}

		[PunRPC]
		private void start_annim()
		{
			Caller.StartAnimation ();
		}


//		void OnGUI()
//		{
//			if (!annim_started)
//			{
//				float h = 30;
//				float w = 200;
//				GUI.Box(new Rect(0, 0, w, h), "Timer : " + (time.Time_remaining).ToString("0"));
//			}
//		}

		protected override void instantiate_team()
		{
			Team team_0 = Game.Instance.Teams[0];
			bool isMine = (PhotonNetwork.isMasterClient);
			int cote = -1; // -1 => domicile | 1 => extérieur
			int i = 0;
			foreach (var player_t0 in team_0.Players)
			{
				// --- Calcule des coordonnées
				int x = team_0.Compo.GetPosition(i)[0];
				int y = team_0.Compo.GetPosition(i)[1];

				float posX = ((-6) + x * 5) / 1.3f;
				float posY = (cote * 22 + (y * 3 * -cote)) / 1.3f;
				// --
				GameObject play0 = Resources.Load("Prefabs/Solo/" + player_t0.UID) as GameObject;
				if (play0 == null)
					play0 = Instantiate (player1_prefab, new Vector3 (posX, 1F, posY), Quaternion.identity) as GameObject;
				else
					play0 = Instantiate (play0, new Vector3 (posX, 1F, posY), Quaternion.identity) as GameObject;
				player_t0.Team_id = 0;
				player_t0.Name += "-" + i;
				play0.name = player_t0.Name + "-" + player_t0.Team_id;
				play0.GetComponent<PhotonView>().viewID = 100 + (++i);
				var controller = play0.AddComponent (this.playerController);
				((BasicPlayerController)controller).Player = player_t0;
				((BasicPlayerController)controller).IsMine = isMine;
			}
			Team team_1 = Game.Instance.Teams[1];
			i = 0;
			foreach (var player_t1 in team_1.Players)
			{
				// --- Calcule des coordonnées
				int x = team_1.Compo.GetPosition(i)[0];
				int y = team_1.Compo.GetPosition(i)[1];

				float posX = ((-6) + x * 5) / 1.3f;
				float posY = (cote * 22 + (y * 3 * -cote)) / 1.3f;
				// ---
				GameObject play1 = Resources.Load("Prefabs/Solo/" + player_t1.UID) as GameObject;
				if (play1 == null)
					play1 = Instantiate(player2_prefab, new Vector3(posX, 1F, -posY), Quaternion.identity) as GameObject;
				else
					play1 = Instantiate (play1, new Vector3(posX, 1F, -posY), Quaternion.identity) as GameObject;
				player_t1.Team_id = 1;
				player_t1.Name += "-" + (i);
				play1.name = player_t1.Name + "-" + player_t1.Team_id;
				play1.GetComponent<PhotonView>().viewID = 200 + (++i);
				var controller = play1.AddComponent (this.playerController);
				((BasicPlayerController)controller).Player = player_t1;
				((BasicPlayerController)controller).IsMine = !isMine;
			}
			GameObject ball = Instantiate(ball_prefab, new Vector3(0, 0.5F, -0), Quaternion.identity) as GameObject;
			ball.name = "Ball";
			ball.GetComponent<PhotonView>().viewID = 300;
		}
	}

	public enum End
	{
		ABANDON,
		TIME
	}
}
