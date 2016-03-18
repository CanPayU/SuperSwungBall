using UnityEngine;
using System.Collections.Generic;

namespace GameScene.Solo
{
    public class Player_controller : MonoBehaviour
    {
        [SerializeField]
        private GameObject Menu;
        [SerializeField]
        private int team_id;
        private Player player; //gere les stats

        // Event Click
        private bool menuDisplayed;
        private RaycastHit hit;

        //Evite le "GetComponent<>"
        private Collider myCollider;
        private MenuController menuController;

        //phaseAnimation
        private bool phaseAnimation;
        private float speed = 0;
        private Vector3 arrivalPoint;
        private bool movement;
        private float pause = 0; // temps de pause du deplacement (en s), quand le player se fait plaquer par exemple

        //Pointeur
        private bool mouseState = false;

        //passe
        private Vector3 arrivalPointPasse;


        #region Getters / Setters
        public Player Player
        { get { return player; } }

        public bool PhaseAnimation
        { get { return phaseAnimation; } }

        public float Pause
        { set { pause = value; } }
        #endregion

        void Start()
        {
            //initialisation menu
            Menu = Instantiate(Menu, new Vector3(), Quaternion.Euler(0, -90, 0)) as GameObject;
            Menu.transform.parent = transform;
            menuController = Menu.GetComponent<MenuController>();

            player = new Player(5, 5, 15, 5, gameObject.name, team_id); // A changer en fonction des stats initiales du perso

            Team t_ = Game.Instance.Teams[team_id];
            t_.add_player(player);
            myCollider = GetComponent<Collider>();

            phaseAnimation = false;
            movement = false;
        }

        void Update()
        {
            if (!phaseAnimation)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit, 100);
                if (Input.GetMouseButtonDown(0) && !hit.Equals(null)) //s'active au clic
                {
                    if (hit.collider == myCollider || hit.collider.transform.parent == Menu.transform) //Activation au clic sur le player ou sur le menu
                    {
                        if (!menuDisplayed) //S'active uniquement le premier clic
                        {
                            player.computeStats(); // Calcule les Stats du perso ( obligé avant d'utiliser un getter)
                            menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse); // Change la taille des zones
                            menuController.display(true);
                        }
                        menuDisplayed = true;
                        mouseState = menuController.set_target(hit);//renvoit true si le joueur clic sur un pointeur et set le 'target' / 'zone_target' aux 'pointeur' / 'zone_du_pointeur' du menu_controller (false et null sinon)
                    }
                    else //Activation lors du clic en dehors du player / menu
                    {
                        menuDisplayed = false;
                        menuController.display(false);
                        mouseState = false; // pas obligé, mais on sait jamais
                    }
                }
                if (Input.GetMouseButtonUp(0))// Clic relache
                {
                    mouseState = false;
                }
                if (mouseState)// s'active tant que le joueur drag and drop un pointeur
                {
                    menuController.move_target(hit);//bouge le pointeur 'target' du menu. Si le target sort de la 'zone_target', replace le 'target'
                }
            }
            else
            {
                // Phase D'animation
                if (pause == 0)
                {
                    if (movement)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, arrivalPoint, speed * Time.deltaTime);
                        if (transform.position == arrivalPoint)
                        {
                            //arret du personnage ( a atteint son point d'arrivé)
                            transform.FindChild("perso").GetComponent<Animator>().Play("Repos");
                            movement = false;
                        }
                    }
                }
                else
                {
                    pause -= Time.deltaTime;
                    if (pause < 0) // pause terminée
                    {
                        pause = 0;
                        if (movement) // play precedente animation
                        {
                            transform.FindChild("perso").LookAt(new Vector3(arrivalPoint.x, transform.FindChild("perso").position.y, arrivalPoint.z));
                            transform.FindChild("perso").GetComponent<Animator>().Play("Course");
                        }
                        else
                        {
                            transform.FindChild("perso").GetComponent<Animator>().Play("Repos");
                        }
                    }
                }
            }
        }

        public void updateValuesPlayer(Color c) //Activation clic boutton
        {
            player.updateValues(convertColorToValue(c)); // Change les Stats du player 
            menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse); // Change la tailles des zones
        }
        private string convertColorToValue(Color c)
        {
            List<Color> colors = menuController.GetButtonsColor;
            if (c == colors[0])
                return "esquive";
            if (c == colors[1])
                return "tacle";
            if (c == colors[2])
                return "passe";
            return "course";
        }

        public bool passe(ref Vector3 pointPasse) // Passe
        {
            pointPasse = arrivalPointPasse;
            return (phaseAnimation && Vector3.Distance(arrivalPointPasse, transform.position) < player.ZonePasse * 5);
        }

        public void start_Anim() // debut de l'animation
        {
            mouseState = false;
            phaseAnimation = true;
            menuController.display(false);
            menuDisplayed = false;
            player.computeStats();
            speed = player.Speed; // vitesse du joueur
            arrivalPoint = new Vector3(menuController.Get_Coordsdeplacement[0], transform.position.y, menuController.Get_Coordsdeplacement[1]); // point d'arrivé du déplacement
            arrivalPointPasse = new Vector3(menuController.Get_CoordsPasse[0], 0.2f, menuController.Get_CoordsPasse[1]); // point d'arrivé de la passe
            pause = 0;

            //animation course
            if (arrivalPoint != transform.position)
            {
                transform.FindChild("perso").LookAt(new Vector3(arrivalPoint.x, transform.FindChild("perso").position.y, arrivalPoint.z));
                transform.FindChild("perso").GetComponent<Animator>().Play("Course");
                movement = true;
            }

        }
        public void end_Anim() // fin de l'animation
        {
            menuController.reset();
            player.reset();
            menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse);
            phaseAnimation = false;
            speed = 0;
            pause = 0;

            //animation repos
            transform.FindChild("perso").GetComponent<Animator>().Play("Repos");
            movement = false;
        }
    }
}