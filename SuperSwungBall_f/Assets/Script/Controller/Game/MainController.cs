using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GameScene
{
    public class MainController : MonoBehaviour
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

		private PhotonPlayer local_player;

        // Use this for initialization
        void Start()
        {
			time = new Timer(5.0F, end_time);
			local_player = PhotonNetwork.player;
            instantiate_team();
			config_goal ();
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
            {
				PhotonView pv = PhotonView.Get (this);
				pv.RPC ("start_annim", PhotonTargets.All);
            }
            time.update();
        }

		[PunRPC] private void end_time()
        {
            time.reset();

            if (annim_started)
            {
				Dictionary<int, Team> teams = Game.Instance.Teams;
				foreach (KeyValuePair<int,Team> team in teams)
                {
					team.Value.end_move_players();
                }
                annim_started = false;
                time.start();
                Debug.Log("Start reflexion");
            }
            else
            {
                annim_started = true;
				PhotonView pv = PhotonView.Get (this);
				pv.RPC ("start_annim", PhotonTargets.All);
            }
        }

		[PunRPC] private void start_annim()
        {
            annim_started = true;
            time.start();

			Dictionary<int, Team> teams = Game.Instance.Teams;
			foreach (KeyValuePair<int,Team> team in teams)
            {
				team.Value.start_move_players();
            }
        }

        void OnGUI()
        {
            if (!annim_started)
            {
                float h = 30;
                float w = 200;
                Rect r = new Rect(0, 0, Screen.width, h);
                Vector2 v = r.center;
                GUI.Box(new Rect(0, 0, w, h), "Timer : " + (time.Time_remaining).ToString("0"));
            }
        }

        public void update_score()
        {
			int other_id = PhotonNetwork.otherPlayers [0].ID;
			Team t_a = Game.Instance.Teams[local_player.ID];
            Team t_b = Game.Instance.Teams[other_id];
            score.text = t_a.Points + " : " + t_b.Points;
        }


        private void instantiate_team()
        {
			float pos = ((local_player.isMasterClient) ? -1 : 1) * 9;
			int nb_player = Game.Instance.Teams[local_player.ID].Nb_Player;
			bool b = local_player.isMasterClient;
			int nb_instance = nb_player - (Convert.ToInt16 (b));
			for (int i = 0; i < nb_instance; i++)
            {
				GameObject play1 = PhotonNetwork.Instantiate (player2_prefab.name, new Vector3 ((float)i * 2, (float)0.5, pos), Quaternion.identity, 0) as GameObject;
                //GameObject play1 = Instantiate(player1_prefab, new Vector3((float)i * 2, (float)0.5, 7), Quaternion.identity) as GameObject;
				//GameObject play2 = PhotonNetwork.Instantiate(player2_prefab.name, new Vector3((float)i * 2, (float)0.5, -7), Quaternion.identity, 0) as GameObject;
				play1.name = local_player.name + i;
                //play2.name = "team2-" + i;
            }
			if (b) {
				GameObject play1 = PhotonNetwork.Instantiate ("Captain", new Vector3 ((float)nb_instance * 2, (float)0.5, pos), Quaternion.identity, 0) as GameObject;
				play1.name = local_player.name + "_Captain";
			}
        }

		private void config_goal(){
			GoalController goal_master = GameObject.Find ("Goal_masterClient").GetComponent<GoalController>();
			GoalController goal_enemy = GameObject.Find ("Goal_enemy").GetComponent<GoalController>();

			int result = -1;
			for (int i = 0; i < PhotonNetwork.playerList.Length && result < 0; i++) {
				PhotonPlayer pp = PhotonNetwork.playerList[i];
				if (!pp.isMasterClient) {
					result = pp.ID;
				}
			}

			// inversé car on ne marque pas dans son but mais l'autre
			goal_master.Team = result;
			goal_enemy.Team = PhotonNetwork.masterClient.ID;

		}
			
		void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
		{
			if(!Game.Instance.isFinish)
				GetComponent<EndController> ().on_end (End.ABANDON, otherPlayer);
		}
    }

	public enum End {
		ABANDON,
		TIME
	}
		

}
