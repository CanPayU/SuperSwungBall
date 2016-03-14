﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GameScene.Solo
{
    public class Main_Controller : MonoBehaviour
    {

        private bool annim_started = false;

        [SerializeField]
        private GameObject player1_prefab;
        [SerializeField]
        private GameObject player2_prefab;
        [SerializeField]
        private Text score;

        CameraController cameraController;

        Text myGuiText;

        Timer time;

        // Use this for initialization
        void Start()
        {
            cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
            time = new Timer(10.0F, end_time);
            instantiate_team();
            //update_score ();
        }

        // Update is called once per frame
        void Update()
        {

            if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
            {
                start_annim();
            }
            time.update();

            if (Input.GetKeyDown(KeyCode.S) && !annim_started)
            {
                int score = 15;
                Debug.Log(User.Instance.score);
                HttpController controller = gameObject.GetComponent<HttpController>();
                controller.sync_score(score, (success) => {
                    Debug.Log(success);
                    Debug.Log(User.Instance.score);
                });
                Debug.Log("sended");
            }

            if (Input.GetKeyDown(KeyCode.C) && !annim_started)
            {
                // localhost
                string username = "antoine"; // id = 1
                string password = "mdp"; // OK
                Debug.Log("isConnected ? " + User.Instance.is_connected);
                HttpController controller = gameObject.GetComponent<HttpController>();
                controller.connect(username, password, (success) => {
                    Debug.Log("isConnected ? " + User.Instance.is_connected + " - Success ?" + success);
                });
                Debug.Log("sended");
            }
        }

        private void suc(bool success)
        {
            Debug.Log(success);
        }

        private void end_time()
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
                cameraController.end_anim();
            }
            else
            {
                annim_started = true;
                start_annim();
            }
        }

        private void start_annim()
        {
            annim_started = true;
            time.start();


			Dictionary<int, Team> teams = Game.Instance.Teams;
			foreach (KeyValuePair<int,Team> team in teams)
			{
				team.Value.start_move_players();
			}
            cameraController.start_anim();
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
            Team t_a = Game.Instance.Teams[0];
            Team t_b = Game.Instance.Teams[1];
            score.text = t_a.Points + " : " + t_b.Points;
        }


        private void instantiate_team()
        {
			int nb_player = Game.Instance.Teams[0].Nb_Player;

			int i = 0;
			foreach (var player in Game.Instance.Teams[0].Players) {
				GameObject play1 = Instantiate(player1_prefab, new Vector3((float)(++i) * 2, 1, 7), Quaternion.identity) as GameObject;
				player.Team_id = 0;
				play1.name = player.Name+"-"+player.Team_id;
			}
			i = 0;
			foreach (var player_t2 in Game.Instance.Teams[1].Players) {
				GameObject play2 = Instantiate(player2_prefab, new Vector3((float)(++i) * 2, 1, -7), Quaternion.identity) as GameObject;
				player_t2.Team_id = 1;
				play2.name = player_t2.Name+"-"+player_t2.Team_id;
			}
        }
    }

}