using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GameScene.Multi
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
		public Timer Time{
			get{ return Time; }
		}

		private PhotonPlayer local_player;
		private PhotonPlayer other_player;

        // Use this for initialization
        void Start()
        {
			Game.Instance = new Game ();
			time = new Timer(5.0F, end_time);
			local_player = PhotonNetwork.player;
			other_player = PhotonNetwork.otherPlayers [0];
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
			Team t_a = Game.Instance.Teams[local_player.ID];
			Team t_b = Game.Instance.Teams[other_player.ID];
            score.text = t_a.Points + " : " + t_b.Points;
        }


        private void instantiate_team()
        {
			int cote = ((local_player.isMasterClient) ? -1 : 1); // coté de l'instanciation

			Team team = Game.Instance.Teams [local_player.ID];

			int nb_player = team.Nb_Player;
			bool b = local_player.isMasterClient;
			int nb_instance = nb_player - (Convert.ToInt16 (b));

			string namePrefab = "";
			if (b) {
				namePrefab = player1_prefab.name;
			}else{
				namePrefab = player2_prefab.name;
			}

			for (int i = 0; i < nb_instance; i++)
            {
				// --- Calcule des coordonnées
				int x = team.Compo.GetPosition (i) [0];
				int y = team.Compo.GetPosition (i) [1];

				float posX = (-12) + x * 5;
				float posY = cote * 20 + (y * 3 * -cote);
				// ---

				GameObject player = PhotonNetwork.Instantiate (namePrefab, new Vector3 (posX, 0.5F, posY), Quaternion.identity, 0) as GameObject;
				Player pl = team.Players [i];
				pl.Name += "-" + i;
				player.name = pl.Name+"-"+pl.Team_id;
            }
			if (b) {
				// --- Calcule des coordonnées
				int x = team.Compo.GetPosition (nb_instance) [0];
				int y = team.Compo.GetPosition (nb_instance) [1];

				float posX = (-12) + x * 5;
				float posY = cote * 20 + (y * 3 * -cote);
				// ---
				GameObject player = PhotonNetwork.Instantiate ("Captain", new Vector3 (posX, 0.5F, posY), Quaternion.identity, 0) as GameObject;
				Player pl = team.Players [nb_instance];
				pl.Name += "-" + nb_instance;
				player.name = pl.Name+"-"+pl.Team_id;
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
    }

	public enum End {
		ABANDON,
		TIME
	}
}
