using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Create_Team
{
    public class MainController : MonoBehaviour
    {

        [SerializeField]
        private GameObject create_panel;
        [SerializeField]
        private GameObject info_panel;
        [SerializeField]
        private GameObject chooseCompo_panel;
        [SerializeField]
        private InputField name_field;

        private System.Random rand = new System.Random();

        void Start()
        {
            GameObject game_panel = GameObject.Find("Game");
            RectTransform rt = game_panel.GetComponent<RectTransform>();
            int w = Screen.width; int h = Screen.height;
			rt.offsetMax = new Vector2( -(w/2), rt.offsetMax.y);
        }

        public void Validate_Creation()
        {
            string name = name_field.text;
            if (name != "")
            {
                string code_name = name.Replace(" ", "_");
                int r_alea = rand.Next(1000);
                Team t = new Team(name_field.text, null, null, code_name + r_alea);
                for (int i = 0; i < t.Nb_Player; i++)
                {
                    t.add_player(new Player(3, 3, 3, 3, "Static", null));
                }
                Settings.Instance.AddOrUpdate_Team(t);
            }
            create_panel.SetActive(false);
            chooseCompo_panel.SetActive(false);
            info_panel.SetActive(true);
            info_panel.GetComponent<TeamController>().Get_Teams_Array();
        }

        public void Create_Team()
        {
            GameObject game_panel = GameObject.Find("Game");
            foreach (Transform child in game_panel.transform)
                Destroy(child.gameObject);
            info_panel.SetActive(false);
            chooseCompo_panel.SetActive(false);
            create_panel.SetActive(true);
        }

        public void Choose_Compo()
        {
            GameObject game_panel = GameObject.Find("Game");
            foreach (Transform child in game_panel.transform)
                Destroy(child.gameObject);
            info_panel.SetActive(false);
            create_panel.SetActive(false);
            chooseCompo_panel.SetActive(true);
        }

        public void Validate_Choose_Compo()
        {
            Composition compo = chooseCompo_panel.GetComponent<ChooseCompoController>().GetChoice();
            info_panel.GetComponent<TeamController>().Compo = compo;
            create_panel.SetActive(false);
            chooseCompo_panel.SetActive(false);
            info_panel.SetActive(true);
        }
    }

}
