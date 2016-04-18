using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
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
        private GameObject ball_prefab;
        [SerializeField]
        private Text score;
        [SerializeField]
        private GUISkin skin;

        InfoJoueurController infoJoueur; // Panel info joueur

        Text myGuiText;
        Timer time;
        public Timer Time
        { get { return Time; } }

        private PhotonPlayer other_player;

        private CameraController cameraController;

        // Use this for initialization
        void Start()
        {
            time = new Timer(10.0F, end_time);
            other_player = PhotonNetwork.otherPlayers[0];
            Team ennemy_t = (Team)other_player.allProperties["Team"];
            Game.Instance = new Game(ennemy_t);
            infoJoueur = GameObject.Find("Canvas").transform.FindChild("InfoJoueur").GetComponent<InfoJoueurController>();
            cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
            instantiate_team();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
            {
                // Sync StartAnnim
                PhotonView pv = PhotonView.Get(this);
                pv.RPC("start_annim", PhotonTargets.All);
            }
            time.update();
        }


        [PunRPC]
        private void end_time()
        {
            // if  is MasterClient
            time.reset();
            if (annim_started)
            { // Start Reflexion
                Dictionary<int, Team> teams = Game.Instance.Teams;
                foreach (KeyValuePair<int, Team> team in teams)
                {
                    team.Value.end_move_players();
                }
                annim_started = false;
                time.start();
                cameraController.end_anim();
            }
            else { // Start annimation
                annim_started = true;
                // Sync StartAnnim
                PhotonView pv = PhotonView.Get(this);
                pv.RPC("start_annim", PhotonTargets.All);
            }
        }

        [PunRPC]
        private void start_annim()
        {
            annim_started = true;
            time.start();
            Dictionary<int, Team> teams = Game.Instance.Teams;
            foreach (KeyValuePair<int, Team> team in teams)
            {
                team.Value.start_move_players();
            }
            infoJoueur.Close();
            cameraController.start_anim();
        }


        void OnGUI()
        {
            GUI.skin = skin;
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
                // ---

                GameObject play0 = Instantiate(player1_prefab, new Vector3(posX, 1F, posY), Quaternion.identity) as GameObject;
                player_t0.Team_id = 0;
                player_t0.Name += "-" + i;
                play0.name = player_t0.Name + "-" + player_t0.Team_id;
                play0.GetComponent<PhotonView>().viewID = 100 + (++i);
                PlayerController controller = play0.GetComponent<PlayerController>();
                controller.Player = player_t0;
                controller.IsMine = isMine;
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

                GameObject play1 = Instantiate(player2_prefab, new Vector3(posX, 1F, -posY), Quaternion.identity) as GameObject;
                player_t1.Team_id = 1;
                player_t1.Name += "-" + (i);
                play1.name = player_t1.Name + "-" + player_t1.Team_id;
                play1.GetComponent<PhotonView>().viewID = 200 + (++i);
                PlayerController controller = play1.GetComponent<PlayerController>();
                controller.Player = player_t1;
                controller.IsMine = !isMine;
            }
            GameObject ball = Instantiate(ball_prefab, new Vector3(0, 0.5F, -0), Quaternion.identity) as GameObject;
            ball.name = "Ball";
            ball.GetComponent<PhotonView>().viewID = 300;
        }

        private void config_goal()
        {
            GoalController goal_master = GameObject.Find("Goal_masterClient").GetComponent<GoalController>();
            GoalController goal_enemy = GameObject.Find("Goal_enemy").GetComponent<GoalController>();

            int result = -1;
            for (int i = 0; i < PhotonNetwork.playerList.Length && result < 0; i++)
            {
                PhotonPlayer pp = PhotonNetwork.playerList[i];
                if (!pp.isMasterClient)
                {
                    result = pp.ID;
                }
            }
            // inversé car on ne marque pas dans son but mais l'autre
            goal_master.Team = result;
            goal_enemy.Team = PhotonNetwork.masterClient.ID;
        }
    }

    public enum End
    {
        ABANDON,
        TIME
    }
}
