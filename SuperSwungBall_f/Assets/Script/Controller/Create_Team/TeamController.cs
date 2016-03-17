using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;


namespace Create_Team
{

    public class TeamController : MonoBehaviour
    {

        [SerializeField]
        private GameObject btn_player;
        [SerializeField]
        private Text team_name;
        [SerializeField]
        private GameObject player_panel;
        [SerializeField]
        private GameObject not_selected;
        private GameObject game_panel;

        private Team[] teams;
        private int index = 0;

        private Team actual_team;
        private Player[] players;
        private int actual_player_id;

        // Use this for initialization
        void Start()
        {
            game_panel = GameObject.Find("Game");
            Get_Teams_Array();
            actual_team = teams[index];
            players = actual_team.Players;
            team_name.text = actual_team.Name;
            not_selected.SetActive(true);
            player_panel.SetActive(false);
            Instantiate_Team();
        }

        public void Get_Teams_Array()
        { // puilc car appelé dans le MainController
            Dictionary<string, Team> teams_dict = Settings.Instance.Default_Team;
            teams = new Team[teams_dict.Count];
            int i = 0;
            foreach (KeyValuePair<string, Team> t in teams_dict)
            {
                teams[i] = t.Value;
                i++;
            }
        }
        private void UpdateColor(int? index_ = null)
        {
            foreach (Transform child in game_panel.transform)
                child.GetComponent<Image>().color = new Color(1, 1, 1);
            if (index_ != null)
            {
                Debug.Log("Player-" + index_);
                Transform btn = game_panel.transform.Find("Player-" + index_);
                btn.GetComponent<Image>().color = new Color(0, 0, 0);
            }
        }
        private void Instantiate_Team()
        {
            foreach (Transform child in game_panel.transform)
                Destroy(child.gameObject);
            int i = 0;
            Debug.Log(actual_team.Compo.Name);
            foreach (Player p in players)
            {
                GameObject btn = Instantiate(btn_player);
                btn.transform.parent = game_panel.transform;
                btn.name = "Player-" + i;
                btn.GetComponent<Button>().onClick.AddListener(() => { Select(); });
                RectTransform rt = btn.GetComponent<RectTransform>();

                // --- Calcule des coordonnées
                int x = actual_team.Compo.GetPosition(i)[0];
                int y = actual_team.Compo.GetPosition(i)[1];
                int w = Screen.width; int h = Screen.height;

                float cst_x = (x + 0.5F) * (0.075f * w); // Proportion
                float cst_y = (y + 0.5F) * (0.145f * h); // Proportion
                rt.anchoredPosition = new Vector2(w * 0.05F + cst_x, -(h * 0.066F + cst_y));
                // ---

                i++;
            }
        }

        public void Save_Team()
        {
            actual_team.Players = players;
            Settings.Instance.AddOrUpdate_Team(actual_team);
            SaveLoad.save_setting();
            Get_Teams_Array();
        }

        public void update_player(Player p)
        {
            players[actual_player_id] = p;
        }

        public void Next()
        {
            if (index < teams.Length - 1)
                index++;
            else
                index = 0;

            actual_team = teams[index];
            players = actual_team.Players;
            team_name.text = actual_team.Name;
            not_selected.SetActive(true);
            player_panel.SetActive(false);
            Instantiate_Team();
        }
        public void Previous()
        {
            if (index > 0)
                index--;
            else
                index = teams.Length - 1;

            actual_team = teams[index];
            players = actual_team.Players;
            team_name.text = actual_team.Name;
            not_selected.SetActive(true);
            player_panel.SetActive(false);
            Instantiate_Team();
        }
        public void Select()
        {
            string name = EventSystem.current.currentSelectedGameObject.name;
            int index = name.IndexOf("-");
            int player_id = int.Parse(name.Substring(index + 1));

            actual_player_id = player_id;
            UpdateColor(player_id);
            Player p = players[player_id];
            player_panel.GetComponent<PlayerController>().Selected(p);
            not_selected.SetActive(false);
            player_panel.SetActive(true);
        }


        public Composition Compo
        {
            set
            {
                actual_team.Compo = value;
                Instantiate_Team();
                Save_Team();
            }
        }
    }

}
