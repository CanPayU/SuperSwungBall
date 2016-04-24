using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Create_Team
{

    public class PlayerController : MonoBehaviour
    {

        [SerializeField]
        private GameObject passe_stat;
        [SerializeField]
        private GameObject course_stat;
        [SerializeField]
        private GameObject tacle_stat;
        [SerializeField]
        private GameObject esquive_stat;
        [SerializeField]
        private Text player_name;

        private Player actual_player;
        private int index = 0;
        private Player[] players;

        void Start()
        {
            Dictionary<string, Player> players_dict = Settings.Instance.Default_player;
            players = new Player[players_dict.Count];
            int i = 0;
            foreach (KeyValuePair<string, Player> p in players_dict)
            {
                players[i] = p.Value; i++;
            }
        }

        private void update_stat()
        {
            player_name.text = actual_player.Name;

			Scrollbar sb = passe_stat.GetComponent<Scrollbar>();
			sb.size = actual_player.PasseBase / 10f;

			sb = course_stat.GetComponent<Scrollbar>();
			sb.size = actual_player.SpeedBase / 10f;

			sb = tacle_stat.GetComponent<Scrollbar>();
			sb.size = actual_player.TacleBase / 10f;

			sb = esquive_stat.GetComponent<Scrollbar>();
			sb.size = actual_player.EsquiveBase / 10f;

        }

        public void send_player_to_team()
        {
            transform.parent.GetComponent<TeamController>().update_player(actual_player);
        }

        public void Selected(Player p)
        {
            actual_player = p;
            update_stat();
        }

        public void Next()
        {
            if (index < players.Length - 1)
                index++;
            else
                index = 0;
            actual_player = players[index];
            player_name.text = actual_player.Name;
            update_stat();
        }
        public void Previous()
        {
            if (index > 0)
                index--;
            else
                index = players.Length - 1;
            actual_player = players[index];
            player_name.text = actual_player.Name;
            update_stat();
        }
    }

}