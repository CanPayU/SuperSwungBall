using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using GameKit;
using GameScene.Replay;
using GameScene.Multi.Replay;

namespace GameScene
{
    public class BasicPlayerController : GameBehavior
    {
        [SerializeField]
        protected GameObject Menu;
        [SerializeField]
        protected int team_id;

        protected Player player; //gere les stats
        protected bool isMine; // je peux le controller ?

        // Event Click
        protected bool menuDisplayed;
        protected RaycastHit hit;
        protected Renderer selection; //cercle de sélection lors du passage de la souris
        protected InfoJoueurController infoJoueur; // Panel info joueur

        //Evite le "GetComponent<>"
        protected Collider myCollider;
        protected MenuController menuController;
        protected CollisionController myCollisionController;

        //phaseAnimation
        protected bool phaseAnimation;
        protected float speed = 0;
        protected Vector3 arrivalPoint;
        protected bool movement;
        protected float pause = 0; //temps de pause du deplacement (en secondes), quand le player se fait plaquer par exemple
        protected Animator anim; //évite le getComponent - anim.Play("animation_name") pour jouer une animation
        protected string animationDeplacement; //nom de l'animation de déplacement du player. Varie en fonction de la vitesse

        //Pointeur
        protected bool mouseState = false; //clic souris enfoncée
        protected FlecheController flecheController; //fleche de déplacement (suit le pointeur de deplacement)
        protected GameObject limiteTerrain; //limites autours du terrain

        //passe
        protected Vector3 arrivalPointPasse;



        #region Getters / Setters
        public bool PhaseAnimation
        { get { return phaseAnimation; } }

        public float Pause
        { set { pause = value; } }
        public Player Player
        {
            get { return player; }
            set { player = value; }
        }
        public bool IsMine
        {
            get { return isMine; }
            set { isMine = value; }
        }
        public Vector3 PointPasse
        {
            get
            {
                arrivalPointPasse = new Vector3(menuController.Get_CoordsPasse[0], 0.2f, menuController.Get_CoordsPasse[1]);
                return arrivalPointPasse;
            }
            set { arrivalPointPasse = value; }
        }
        public Vector3 PointDeplacement
        {
            get
            {
                arrivalPoint = new Vector3(menuController.Get_Coordsdeplacement[0], transform.position.y, menuController.Get_Coordsdeplacement[1]);
                return arrivalPoint;
            }
            set { arrivalPoint = value; }
        }
        #endregion

        public BasicPlayerController()
        {
            this.eventType = GameKit.EventType.All;
        }

        protected virtual void Start()
        {
            this.Menu = Resources.Load("Prefabs/menu") as GameObject;

            //initialisation menu
            Menu = Instantiate(Menu, new Vector3(), Quaternion.Euler(0, -90, 0)) as GameObject;
            Menu.transform.parent = transform;

            menuController = Menu.GetComponent<MenuController>();
            myCollisionController = GetComponent<CollisionController>();
            selection = transform.FindChild("selection").GetComponent<Renderer>();
            selection.enabled = false;
            infoJoueur = GameObject.Find("Canvas").transform.FindChild("InfoJoueur").GetComponent<InfoJoueurController>();
            flecheController = transform.FindChild("fleche").GetComponent<FlecheController>();


            // A set dans le instanciate (Main)
            //player = new Player(5, 5, 15, 5, gameObject.name, team_id); 
            //isMine = true;

            arrivalPointPasse = new Vector3(0, 0, 0);
            arrivalPoint = new Vector3(0, 0, 0);
            Debug.Log("start");
            myCollider = GetComponent<Collider>();
            limiteTerrain = GameObject.Find("terrain").transform.FindChild("limites").gameObject;

            anim = transform.FindChild("perso").GetComponent<Animator>();
            animationDeplacement = "Course";
            phaseAnimation = false;
            movement = false;
        }
        protected virtual void Update()
        {
            if (!phaseAnimation)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray, out hit, 100);
                if (Input.GetMouseButtonDown(0) && !hit.Equals(null)) //Event CLic
                {
                    if (hit.collider == myCollider || hit.collider.transform.parent == Menu.transform) //Activation au clic sur le player ou sur le menu
                    {
                        if (isMine) // clic sur un allié
                        {
                            infoJoueur.DisplayAlly(player); // Affiche les stats du joueur;
                            if (!menuDisplayed) //S'active uniquement si le menu n'est pas encore affiché
                            {
                                player.computeStats(); //Calcule les Stats du perso ( obligé avant d'utiliser un getter)
                                menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse); // Change la taille des zones
                                menuController.display(true);
                            }
                            menuDisplayed = true;
                            mouseState = menuController.set_target(hit);//renvoit true si le joueur clic sur un pointeur et set le 'target' / 'zone_target' aux 'pointeur' / 'zone_du_pointeur' du menu_controller (false et null sinon)
                            if (mouseState)
                            {
                                limiteTerrain.SetActive(true);//activation des limites du terrain (pour empêcher le pointeur de sortir)
                                Physics.Raycast(ray, out hit, 100);
                                menuController.move_target(hit);
                                flecheController.point(new Vector2(menuController.Get_Coordsdeplacement[0], menuController.Get_Coordsdeplacement[1]));
                            }
                        }
                        else // clic sur un adversaire
                        {
                            infoJoueur.DisplayEnnemy(player);
                        }
                    }
                    else //Activation lors du clic en dehors du player / menu
                    {
                        menuDisplayed = false;
                        menuController.display(false);
                        mouseState = false; //pas obligé, mais on sait jamais
                    }
                }
                if (Input.GetMouseButtonUp(0))//Clic relache
                {
                    mouseState = false;
                    limiteTerrain.SetActive(false);//suppression des limites du terrain (pour pouvoir cliquer sur les boutons présents en dehors)
                }
                if (mouseState)//s'active tant que le joueur drag and drop un pointeur
                {
                    menuController.move_target(hit);//bouge le pointeur 'target' du menu. Si le target sort de la 'zone_target', replace le 'target'
                    flecheController.point(new Vector2(menuController.Get_Coordsdeplacement[0], menuController.Get_Coordsdeplacement[1])); //bouge la flèche de déplacement
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
                            anim.Play("Repos");
                            movement = false;
                        }
                    }
                }
                else
                {
                    pause -= Time.deltaTime;
                    if (pause < 0) // pause terminée
                    {
                        myCollider.enabled = true;
                        pause = 0;
                        if (movement) // play precedente animation
                        {
                            transform.FindChild("perso").LookAt(new Vector3(arrivalPoint.x, transform.FindChild("perso").position.y, arrivalPoint.z));
                            anim.Play(animationDeplacement);
                        }
                        else
                        {
                            anim.Play("Repos");
                        }
                    }
                }
            }
        }
        void OnMouseEnter()
        {
            if (!phaseAnimation)
            {
                selection.enabled = true;
            }
        }
        void OnMouseExit()
        {
            selection.enabled = false;
        }

        public virtual void updateValuesPlayer(Color c) //Activation clic boutton
        {
            menuController.update_Color(c); //change la couleur des trois bouttons "valeur"
            player.updateValues(convertColorToValue(c)); // Change les Stats du player 
            flecheController.changeColor(c); // change les couleurs de la flèche de déplacement
            menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse); // Change la tailles des zones
            flecheController.point(new Vector2(menuController.Get_Coordsdeplacement[0], menuController.Get_Coordsdeplacement[1])); //bouge la flèche de déplacement
        }
        private string convertColorToValue(Color c) //revoit la chaine de caractère liée à la couleur
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
            Debug.Log("In PC:" + Player.Name + " - point:" + arrivalPoint);
            pointPasse = arrivalPointPasse;
            return (phaseAnimation && Vector3.Distance(arrivalPointPasse, transform.position) < player.ZonePasse * 5);
        }
        public void Animation(string animation, float tempsAnimation)
        {
            myCollider.enabled = false;
            pause = tempsAnimation;
            anim.Play(animation);
        }

        public virtual void start_Anim(bool setPoint = true) // debut de l'animation
        {
            myCollisionController.start_anim();
            flecheController.display(false);
            mouseState = false;
            phaseAnimation = true;
            menuController.display(false);
            menuDisplayed = false;
            player.computeStats();
            speed = player.Speed; // vitesse du joueur
            pause = 0;

            if (setPoint)
            {
                if (!isMine)
                    Debug.Log("ERROR");
                arrivalPoint = new Vector3(menuController.Get_Coordsdeplacement[0], transform.position.y, menuController.Get_Coordsdeplacement[1]); // point d'arrivé du déplacement
                arrivalPointPasse = new Vector3(menuController.Get_CoordsPasse[0], 0.2f, menuController.Get_CoordsPasse[1]); // point d'arrivé de la passe
            }

            Debug.Log(arrivalPoint + " -- " + setPoint);

            //animation course
            if (arrivalPoint != transform.position)
            {
                transform.FindChild("perso").LookAt(new Vector3(arrivalPoint.x, transform.FindChild("perso").position.y, arrivalPoint.z));
                animationDeplacement = "Marche";
                if (speed > 1)
                    animationDeplacement = "Course"; // animation de déplacement en fonction de la vitesse

                anim.Play(animationDeplacement);
                movement = true;
            }

        }
        public void end_Anim() // fin de l'animation
        {
            menuController.reset();
            flecheController.display(true);
            flecheController.reset();
            player.reset();
            menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse);
            phaseAnimation = false;
            speed = 0;
            pause = 0;
            selection.enabled = false;

            //animation repos
            anim.Play("Repos");
            movement = false;
        }


        // -- Event

        public override void OnSucceedAttack(Player p)
        {
            Debug.Log("I have succeed my Attack : " + p.Name + " - " + name);
        }
        public override void OnSucceedEsquive(Player p)
        {
            Debug.Log("I Have succeed my Esquive : " + p.Name + " - " + name);
        }
        public override void OnFailedAttack(Player p)
        {
            Debug.Log("I Have failed my Esquive : " + p.Name + " - " + name);
        }
        public override void OnFailedEsquive(Player p)
        {
            Debug.Log("I Have failed my Esquive : " + p.Name + " - " + name);
        }

        public override void OnStartAnimation()
        {
            start_Anim();
        }
        public override void OnStartReflexion()
        {
            end_Anim();
        }



        // -- En développement
        private void GetMyParam(PlayerAction action)
        {
            this.PointDeplacement = action.Deplacement;
            this.PointPasse = action.Passe;
            this.Player.Button_Values = action.ButtonValues;
            start_Anim(false);
        }
        // --
    }
}