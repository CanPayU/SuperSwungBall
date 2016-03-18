using UnityEngine;
using System.Collections.Generic;

namespace Didacticiel
{
    public class MenuController : MonoBehaviour
    {
        private Dictionary<string, GameObject[]> components = new Dictionary<string, GameObject[]>(); // permet de faciliter l'accès aux composants du menu ex : components["buttons"][0]
        private List<Color> buttonsColor = new List<Color>(); // Facilite l'accès des couleurs des boutons depuis le player_controller
        private GameObject target; // pointeur selectionné

        #region getters
        public float[] Get_Coordsdeplacement //renvoit les coordonnées du pointeur déplacement
        {
            get { return new float[2] { components["pointeurs"][0].transform.position.x, components["pointeurs"][0].transform.position.z }; }
        }
        public float[] Get_CoordsPasse //renvoit les coordonnées du pointeur passe
        {
            get { return new float[2] { components["pointeurs"][1].transform.position.x, components["pointeurs"][1].transform.position.z }; }
        }
        public List<Color> GetButtonsColor
        {
            get { return buttonsColor; }
        }
        #endregion

        void Start()
        {
            initialize_components(); //add les composants à "components" et set les couleurs des boutons
            transform.localPosition = new Vector3(0, 0, 0);
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            display(false);
        }

        # region menu
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
            add_components("pointeurs", 9, 11);

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
            components["zones"][0].GetComponent<Renderer>().material.color = new Color(0.6f, 1, 1); // deplacement
            components["zones"][1].GetComponent<Renderer>().material.color = new Color(1, 0.6f, 1); // passe

            //Couleur pointeurs
            components["pointeurs"][0].GetComponent<Renderer>().material.color = new Color(0.2f, 0.7f, 0.7f); // deplacement
            components["pointeurs"][1].GetComponent<Renderer>().material.color = new Color(0.7f, 0.2f, 0.7f); // passe
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
        public void reset()
        {
            components["zones"][0].transform.localScale = new Vector3(0, components["zones"][0].transform.localScale.y, 0);
            components["zones"][1].transform.localScale = new Vector3(0, components["zones"][1].transform.localScale.y, 0);
            components["pointeurs"][0].transform.localPosition = new Vector3(0, components["pointeurs"][0].transform.localPosition.y, 0);
            components["pointeurs"][1].transform.localPosition = new Vector3(0, components["pointeurs"][1].transform.localPosition.y, 0);
            foreach (GameObject valeur in components["valeurs"])
            {
                valeur.GetComponent<Renderer>().material.color = Color.white;
            }
        }
        #endregion
        #region pointeurs
        public void update_zoneDeplacement(float zone_deplacement, float zone_passe) // Modifie la taille des zones
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
            replace_pointeur();
        }
        public bool set_target(RaycastHit hit)//renvoit true si le joueur clic sur un pointeur et set le 'target' / 'zone_target'
        {
            if (hit.collider == components["pointeurs"][0].GetComponent<Collider>() || hit.collider == components["zones"][0].GetComponent<Collider>())
            {
                target = components["pointeurs"][0];
                return true;
            }
            if (hit.collider == components["pointeurs"][1].GetComponent<Collider>() || hit.collider == components["zones"][1].GetComponent<Collider>())
            {
                target = components["pointeurs"][1];
                return true;
            }
            target = null;
            return false;
        }
        public void reset_target()
        {
            target = null;
        }
        public void move_target(RaycastHit hit) // Deplace le target en fonction de la position de la souris
        {
            if (target != null)
            {
                target.transform.position = new Vector3(hit.point.x, target.transform.position.y, hit.point.z);
                if (target == components["pointeurs"][0])
                {
                    replace_pointeur();
                }
            }
        }
        private void replace_pointeur() // replace le pointeur de déplacement dans sa zone si il y sort
        {
            components["pointeurs"][0].transform.LookAt(components["zones"][0].transform);
            float distance = Vector3.Distance(components["pointeurs"][0].transform.position, components["zones"][0].transform.position);
            if (distance > 10 * components["zones"][0].transform.localScale.x / 2)
            {
                float angle = components["pointeurs"][0].transform.eulerAngles.y - transform.rotation.eulerAngles.y;
                components["pointeurs"][0].transform.localPosition = new Vector3(-Mathf.Sin(Mathf.Deg2Rad * angle) * components["zones"][0].transform.localScale.x / 2, components["pointeurs"][0].transform.localPosition.y, -Mathf.Cos(Mathf.Deg2Rad * angle) * components["zones"][0].transform.localScale.x / 2);
            }
        }
        #endregion
    }
}