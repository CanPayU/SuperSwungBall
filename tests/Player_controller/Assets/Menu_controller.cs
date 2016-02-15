using UnityEngine;
using System.Collections.Generic;

namespace Assets
{
    public class Menu_controller : MonoBehaviour
    {
        private Dictionary<string, GameObject[]> components = new Dictionary<string, GameObject[]>(); // permet de faciliter l'accès aux composants du menu ex : components["buttons"][0]
        private List<Color> buttonsColor = new List<Color>(); // Facilite l'accès des couleurs des boutons depuis le player_controller
        void Start()
        {
            initialize_components(); //add les composants à "components" et set les couleurs des boutons
            display(false); // Menu inactif par defaut
        }
        void Update()
        {

        }
        public void display(bool afficher)//affiche/cache le menu
        {
            int nb_Child = transform.childCount;
            for (int i = 0; i < nb_Child; ++i)
            {
                transform.GetChild(i).GetComponent<Collider>().enabled = afficher;
                transform.GetChild(i).GetComponent<Renderer>().enabled = afficher;
            }
        }

        private void initialize_components()//add les composants à "components" et set les couleurs des boutons
        {
            add_components("buttons", 0, 4);
            add_components("zones", 4, 6);
            add_components("valeurs", 6, 9);

            //Changement couleurs boutons
            components["buttons"][0].GetComponent<Renderer>().material.color = new Color(1, 0.5f, 0); //esquive
            components["buttons"][1].GetComponent<Renderer>().material.color = new Color(0, 0.5f, 0); //tacle
            components["buttons"][2].GetComponent<Renderer>().material.color = new Color(1, 0.2f, 0.7f); //passe
            components["buttons"][3].GetComponent<Renderer>().material.color = new Color(0, 0.5f, 1); //course

            buttonsColor.Add(new Color(1, 0.5f, 0));
            buttonsColor.Add(new Color(0, 0.5f, 0));
            buttonsColor.Add(new Color(1, 0.2f, 0.7f));
            buttonsColor.Add(new Color(0, 0.5f, 1));

            //Couleur zones
            components["zones"][0].GetComponent<Renderer>().material.color = new Color(0.5f, 1, 1); // deplacement
            components["zones"][1].GetComponent<Renderer>().material.color = new Color(1, 0.5f, 1); // passe
        }

        private void add_components(string key, int min, int max) // recupere les gameObject enfant du menu de "min" (compris) à "max" (exclu) et les ajoute au dictionnaire "components"
        {
            GameObject[] component = new GameObject[max - min];
            for (int i = min; i < max; ++i)
                component[i - min] = transform.GetChild(i).gameObject;
            components.Add(key, component);
        }
        public void update_Color(Color c) // modifie les couleurs les objets "valeurs"
        {
            components["valeurs"][2].GetComponent<Renderer>().material.color = components["valeurs"][1].GetComponent<Renderer>().material.color;
            components["valeurs"][1].GetComponent<Renderer>().material.color = components["valeurs"][0].GetComponent<Renderer>().material.color;
            components["valeurs"][0].GetComponent<Renderer>().material.color = c;
        }
        public List<Color> GetButtonsColor()
        {
            return buttonsColor;
        }

        public void update_zoneDeplacement(float zone_deplacement, float zone_passe)
        {
            components["zones"][0].transform.localScale = new Vector3(zone_deplacement, 0.005f, zone_deplacement);
            components["zones"][1].transform.localScale = new Vector3(zone_passe, 0.005f, zone_passe);
            if (zone_passe < zone_deplacement)
            {
                components["zones"][0].transform.localPosition = new Vector3(0, 0, 0);
                components["zones"][1].transform.localPosition = new Vector3(0, 0.005f, 0);
            }
            else
            {
                components["zones"][0].transform.localPosition = new Vector3(0, 0.005f, 0);
                components["zones"][1].transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }
}