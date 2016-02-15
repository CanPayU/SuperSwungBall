using UnityEngine;
using System.Collections.Generic;
namespace Assets
{
    public class Player_controller : MonoBehaviour
    {
        [SerializeField]
        private GameObject Menu;
        private Player player; //gere les stats

        // Event Click
        private bool clicked;
        private RaycastHit hit;

        //Evite le "GetComponent<>"
        private Collider myCollider;
        private Menu_controller menuController;

        //deplacement
        private bool deplacement;
        private float speed = 0;

        void Start()
        {
            //initialisation menu
            Menu = Instantiate(Menu, new Vector3(), Quaternion.identity) as GameObject;
            Menu.transform.parent = transform;
            transform.position = new Vector3(2, 2, 2);
            menuController = Menu.GetComponent<Menu_controller>();

            player = new Player(5, 5, 8, 5);

            myCollider = GetComponent<Collider>();
        }
        void Update()
        {
            if (!deplacement)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))//Activation au clic
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        if (hit.collider == myCollider || hit.collider.transform.parent == Menu.transform) //Activation au clic sur le player ou sur le menu
                        {
                            if (!clicked) //S'active uniquement le premier clic
                            {
                                menuController.display(true);
                            }
                            clicked = true;
                        }
                        else //Activation lors du clic en dehors du player / menu
                        {
                            clicked = false;
                            menuController.display(false);
                        }
                    }
                }
            }
            else// phase d'animation
            {

            }
        }
        public void updateValuesPlayer(Color c) //Activation clic boutton
        {
            player.updateValues(convertColorToValue(c)); // Change les Stats du player 
            menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse);
        }

        private string convertColorToValue(Color c)
        {
            List<Color> colors = menuController.GetButtonsColor();
            if (c == colors[0])
                return "tacle";
            if (c == colors[1])
                return "esquive";
            if (c == colors[2])
                return "passe";
            return "course";
        }

        public void start_Anim() // debut de l'animation
        {
            deplacement = true;
            player.computeStats();
            speed = player.Speed;
        }
        public void end_Anim() // fin de l'animation
        {
            deplacement = false;
            speed = 0;
        }
    }
}