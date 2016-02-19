using UnityEngine;
using System.Collections;
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

        // Use this for initialization
        void Start()
        {
            time = new Timer(5.0F, end_time);
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
                Team[] teams = Game.Instance.Teams;
                foreach (Team team in teams)
                {
                    team.end_move_players();
                }
                annim_started = false;
                time.start();
                Debug.Log("Start reflexion");
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

            Team[] teams = Game.Instance.Teams;
            foreach (Team team in teams)
            {
                team.start_move_players();
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
            Team t_a = Game.Instance.Teams[0];
            Team t_b = Game.Instance.Teams[1];
            score.text = t_a.Points + " : " + t_b.Points;
        }


        private void instantiate_team()
        {
            int nb_player = Game.Instance.Teams[0].Nb_Player;
            for (int i = 0; i < nb_player; i++)
            {
				GameObject test = PhotonNetwork.Instantiate (player1_prefab.name, new Vector3 ((float)i * 2, (float)0.5, 7), Quaternion.identity, 0) as GameObject;
                //GameObject play1 = Instantiate(player1_prefab, new Vector3((float)i * 2, (float)0.5, 7), Quaternion.identity) as GameObject;
                GameObject play2 = Instantiate(player2_prefab, new Vector3((float)i * 2, (float)0.5, -7), Quaternion.identity) as GameObject;
				//test.name = "team1-" + i;
                play2.name = "team2-" + i;
            }
        }
    }

}
