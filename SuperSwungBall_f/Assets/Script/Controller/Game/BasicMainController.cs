using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

using GameKit;
using GameScene.Instantiator;

namespace GameScene
{
	public class BasicMainController : GameBehavior {

		protected bool annim_started = false;

		protected GameObject player1_prefab;
		protected GameObject player2_prefab;
		protected Text score;
		protected Text timerText;
		protected Animator timerAnimator;

		protected Text myGuiText;
		protected Timer time;

		protected Type playerController;

		public BasicMainController(){
			this.eventType = GameKit.EventType.Global;
		}

		// Use this for initialization
		protected virtual void Start()
		{
			this.player1_prefab = Resources.Load("Prefabs/Solo/Player_1") as GameObject;
			this.player2_prefab = Resources.Load("Prefabs/Solo/Player_2") as GameObject;
			this.score = GameObject.Find ("PanelScore").transform.Find ("Text").GetComponent<Text> ();
			var gm = GameObject.Find ("Timer");
			this.timerText = gm.transform.GetComponent<Text> ();
			this.timerAnimator = gm.transform.GetComponent<Animator> ();

			time = new Timer(10.0F, end_time);
			instantiate_team();
		}

		// Update is called once per frame
		protected virtual void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
			{
				Caller.StartAnimation ();
			}
			time.update();
		}

		protected virtual void end_time()
		{
			this.time.reset();
			if (annim_started)
				Caller.StartReflexion();
			else
				Caller.StartAnimation();
		}

		public void update_score()
		{
			Team t_a = Game.Instance.Teams[0];
			Team t_b = Game.Instance.Teams[1];
			score.text = t_a.Points + " : " + t_b.Points;
		}


		protected virtual void instantiate_team()
		{
			Team team_0 = Game.Instance.Teams[0];
			int cote = -1; // -1 => domicile | 1 => extérieur
			int i = 0;
			foreach (var player_t0 in team_0.Players)
			{
				// --- Calcule des coordonnées
				int x = team_0.Compo.GetPosition(i)[0];
				int y = team_0.Compo.GetPosition(i)[1];

				float posX = ((-6) + x * 5) / 1.3f;
				float posY = (cote * 22 + (y * 3 * -cote)) / 1.3f;
				// ---
				GameObject play0 = Resources.Load("Prefabs/Solo/" + player_t0.UID) as GameObject;
				if (play0 == null)
					play0 = Instantiate (player1_prefab, new Vector3 (posX, 1F, posY), Quaternion.identity) as GameObject;
				else
					play0 = Instantiate (play0, new Vector3 (posX, 1F, posY), Quaternion.identity) as GameObject;
				player_t0.Team_id = 0;
				player_t0.Name += "-" + (++i);
				play0.name = player_t0.Name + "-" + player_t0.Team_id;
				var controller = play0.AddComponent (this.playerController);
				((BasicPlayerController)controller).Player = player_t0;
				((BasicPlayerController)controller).IsMine = true;
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
				else {
					play1 = Instantiate(play1, new Vector3(posX, 1F, -posY), Quaternion.identity) as GameObject;
					play1.transform.eulerAngles = new Vector3(0, 180, 0);
				}
				player_t1.Team_id = 1;
				player_t1.Name += "-" + (++i);
				play1.name = player_t1.Name + "-" + player_t1.Team_id;
				var controller = play1.AddComponent (this.playerController);
				((BasicPlayerController)controller).Player = player_t1;
				((BasicPlayerController)controller).IsMine = true;
			}
		}

		// ------- Event
		public override void OnGoal(GoalController controller){
			Debug.Log ("OnGoal TeamId: " + controller.Team);
		}
		public override void OnStartAnimation(){
			Debug.Log ("OnStartAnimation");

			this.time = new Timer (8.0F, end_time);
			this.annim_started = true;
			this.time.start ();
		}
		public override void OnStartReflexion(){
			Debug.Log ("Start Reflexion");

			this.annim_started = false;
			this.time = new Timer(60.0F, end_time);
			this.time.start();
		}
		public override void OnEndGame(GameScene.Multi.End type) {
			update_score ();
		}
	}
}