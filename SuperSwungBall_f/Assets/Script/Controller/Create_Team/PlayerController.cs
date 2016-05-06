using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Create_Team
{

    public class PlayerController : MonoBehaviour
    {

		private ScrollValueController passe_stat;
		private ScrollValueController course_stat;
		private ScrollValueController tacle_stat;
		private ScrollValueController esquive_stat;
		private ScrollValueController ducat_stat;
        [SerializeField]
        private Text player_name;

		private GameObject statsPanel;
		private GameObject swungMen;
		private bool statsDisplayed;

		private Button chooseBtn;
		private TeamController controller;

        private Player actualPlayer;
        private int index = 0;
        private Player[] players;

		void Awake(){
			Transform stats = transform.Find ("Stats");
			this.statsPanel = stats.gameObject;
			this.passe_stat = stats.Find ("Passe").GetComponent<ScrollValueController>();
			this.course_stat = stats.Find ("Course").GetComponent<ScrollValueController>();
			this.tacle_stat = stats.Find ("Tacle").GetComponent<ScrollValueController>();
			this.esquive_stat = stats.Find ("Esquive").GetComponent<ScrollValueController>();
			this.ducat_stat = stats.Find ("Ducat").GetComponent<ScrollValueController>();
		}

        void Start()
        {
			this.chooseBtn = transform.Find ("Choose").GetComponent<Button> ();
			this.controller = transform.parent.GetComponent<TeamController> ();

			getArrayOfPlayers ();

			this.chooseBtn.onClick.AddListener (delegate {
				controller.update_player(actualPlayer);
			});
        }

		private void getArrayOfPlayers(){
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
			InstantiateSwungMen ();
			DisplayInformation (false);
			player_name.text = actualPlayer.Name;

			this.passe_stat.Value = actualPlayer.PasseBase;
			this.passe_stat.updateText (10);
			this.course_stat.Value = actualPlayer.SpeedBase;
			this.course_stat.updateText (10);
			this.tacle_stat.Value = actualPlayer.TacleBase;
			this.tacle_stat.updateText (10);
			this.esquive_stat.Value = actualPlayer.EsquiveBase;
			this.esquive_stat.updateText (10);
			this.ducat_stat.Value = actualPlayer.Ducat;
			this.ducat_stat.updateText (10);

			if (!controller.updateDucat (actualPlayer))
				this.chooseBtn.interactable = false;
			else
				this.chooseBtn.interactable = true;
        }

		/// <summary> Affiche les infos sur le SwungMens </summary>
		public void DisplayInformation(bool change = true)
		{
			if (change)
				this.statsDisplayed = !this.statsDisplayed;
			this.statsPanel.SetActive (this.statsDisplayed);
			this.swungMen.SetActive (!this.statsDisplayed);
		}

		private void InstantiateSwungMen(){
			if (this.swungMen != null)
				DestroyImmediate (this.swungMen);

			GameObject gm = Resources.Load("Prefabs/Resources/" + this.actualPlayer.UID) as GameObject;
			this.swungMen = Instantiate(gm);
			Transform player = this.swungMen.transform;
			player.SetParent(transform, false);

			Transform rect = player.GetComponent<Transform>();
			rect.localScale = new Vector3(30f, 30f, 30f);
			rect.localPosition = new Vector3(0, -110f, -20f);
			rect.localRotation = new Quaternion(0, -180f, 0, 0);
		}

        public void Selected(Player p)
        {
			actualPlayer = p;
            update_stat();
        }

        public void Next()
        {
            if (index < players.Length - 1)
                index++;
            else
                index = 0;
			actualPlayer = players[index];
			player_name.text = actualPlayer.Name;
            update_stat();
        }
        public void Previous()
        {
            if (index > 0)
                index--;
            else
                index = players.Length - 1;
			actualPlayer = players[index];
			player_name.text = actualPlayer.Name;
            update_stat();
        }
    }

}