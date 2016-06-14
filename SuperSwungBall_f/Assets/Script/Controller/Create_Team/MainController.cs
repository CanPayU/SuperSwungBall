using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

using TranslateKit;

namespace Create_Team
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private GameObject info_panel;
        [SerializeField]
        private GameObject chooseCompo_panel;

        private System.Random rand = new System.Random();

        void Start()
        {
            GameObject game_panel = GameObject.Find("Game");
            RectTransform rt = game_panel.GetComponent<RectTransform>();
            int w = Screen.width;
			rt.offsetMax = new Vector2( -(w/2), rt.offsetMax.y);
        }

		public void Validate_Creation(string name)
        {
            string code_name = name.Replace(" ", "_");
            int r_alea = rand.Next(1000);
			Composition compo = Settings.Instance.Default_compo.First ().Value;
			Team t = new Team(name, compo, null, code_name + r_alea);
            for (int i = 0; i < t.Nb_Player; i++)
            {
				t.add_player(Settings.Instance.Random_Player);
            }
            Settings.Instance.AddOrUpdate_Team(t);
            
            chooseCompo_panel.SetActive(false);
            info_panel.SetActive(true);
            info_panel.GetComponent<TeamController>().Get_Teams_Array();
        }

        public void Create_Team()
		{
			info_panel.SetActive(false);
			chooseCompo_panel.SetActive(false);
			//create_panel.SetActive(true);

			Notification.Text ("Creer une equipe", "Choisissez le nom de votre équipe.", force: true, completion: (value) => {
				Validate_Creation(value);
			});
        }

        public void Choose_Compo()
        {
            GameObject game_panel = GameObject.Find("Game");
            foreach (Transform child in game_panel.transform)
                Destroy(child.gameObject);
            info_panel.SetActive(false);
            chooseCompo_panel.SetActive(true);
        }

        public void Validate_Choose_Compo()
        {
            Composition compo = chooseCompo_panel.GetComponent<ChooseCompoController>().GetChoice();
            info_panel.GetComponent<TeamController>().Compo = compo;
            chooseCompo_panel.SetActive(false);
            info_panel.SetActive(true);
        }

		public void moreInformation(){
			string title = Language.GetValue (TradValues.CreateTeam.MoreInfo);
			string content = Language.GetValue (TradValues.CreateTeam.MoreInfoDesc);
			Notification.SimpleAlert (title, content);
		}
    }

}
