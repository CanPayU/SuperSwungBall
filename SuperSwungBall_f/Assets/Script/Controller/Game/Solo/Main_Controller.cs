using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using GameKit;

namespace GameScene.Solo
{
	public class Main_Controller : GameBehavior
    {
        private bool annim_started = false;

        [SerializeField]
        private GameObject player1_prefab;
        [SerializeField]
        private GameObject player2_prefab;
        [SerializeField]
        private Text score;

        Text myGuiText;

        Timer time;

		public Main_Controller(){
			this.eventType = GameKit.EventType.Global;
		}

        // Use this for initialization
        void Start()
        {
            time = new Timer(10.0F, end_time);
            instantiate_team();
            //update_score ();
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
            {
				Caller.StartAnimation ();
            }
            time.update();
        }

        private void suc(bool success)
        {
            Debug.Log(success);
        }

        private void end_time()
        {
            this.time.reset();

            if (annim_started)
				Caller.StartReflexion();
            else
				Caller.StartAnimation();
        }

        void OnGUI()
        {
            if (!annim_started)
            {
                float h = 30;
                float w = 200;
                GUI.Box(new Rect(0, 0, w, h), "Timer : " + (time.Time_remaining).ToString("0"));
            }
        }

        public void update_score()
        {
            Team t_a = Game.Instance.Teams[0];
            Team t_b = Game.Instance.Teams[1];
            score.text = t_a.Points + " : " + t_b.Points;
        }


        private void instantiate_team()
		{
			Debug.Log("TeamInfo : default:" + Settings.Instance.Default_Team.ToStringFull());
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
                PlayerController controller = play0.GetComponent<PlayerController>();
                controller.Player = player_t0;
                controller.IsMine = true;
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
                player_t1.Team_id = 1;
                player_t1.Name += "-" + (++i);
                play1.name = player_t1.Name + "-" + player_t1.Team_id;
                PlayerController controller = play1.GetComponent<PlayerController>();
                controller.Player = player_t1;
                controller.IsMine = true;
            }
			Debug.Log("TeamInfo : default:" + Settings.Instance.Default_Team.ToStringFull());
        }

		// ------- Event
		public override void OnGoal(GoalController controller){
			Debug.Log ("OnGoal TeamId: " + controller.Team);
		}
		public override void OnStartAnimation(){
			Debug.Log ("OnStartAnimation");

			this.time = new Timer (10.0F, end_time);
			this.annim_started = true;
			this.time.start ();
		}
		public override void OnStartReflexion(){
			Debug.Log ("Start Reflexion");

			this.annim_started = false;
			this.time = new Timer(10.0F, end_time);
			this.time.start();
		}
    }

}
