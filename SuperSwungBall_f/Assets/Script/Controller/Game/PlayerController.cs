﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

using GameKit;

namespace GameScene
{
	public class PlayerController : GameBehavior
    {
        [SerializeField]
        private GameObject Menu;
        [SerializeField]
        private int team_id;

        private Player player; //gere les stats
        private bool isMine; // je peux le controller ?

        // Event Click
        private bool menuDisplayed;
        private RaycastHit hit;
        private Renderer selection; //cercle de sélection lors du passage de la souris
        InfoJoueurController infoJoueur; // Panel info joueur

        //Evite le "GetComponent<>"
        private Collider myCollider;
        private MenuController menuController;
        private CollisionController myCollisionController;

        //phaseAnimation
        private bool phaseAnimation;
        private float speed = 0;
        private Vector3 arrivalPoint;
        private bool movement;
        private float pause = 0; //temps de pause du deplacement (en secondes), quand le player se fait plaquer par exemple
        private Animator anim; //évite le getComponent - anim.Play("animation_name") pour jouer une animation
        private string animationDeplacement; //nom de l'animation de déplacement du player. Varie en fonction de la vitesse

        //Pointeur
        private bool mouseState = false; //clic souris enfoncée
        private FlecheController flecheController; //fleche de déplacement (suit le pointeur de deplacement)
        private GameObject limiteTerrain; //limites autours du terrain

        //passe
        private Vector3 arrivalPointPasse;

        //Network
        private PhotonView view;


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

		public PlayerController(){
			this.eventType = GameKit.EventType.All;
		}

        void Start()
        {
            //initialisation menu
            Menu = Instantiate(Menu, new Vector3(), Quaternion.Euler(0, -90, 0)) as GameObject;
            Menu.transform.parent = transform;

            menuController = Menu.GetComponent<MenuController>();
            myCollisionController = GetComponent<CollisionController>();
            selection = transform.FindChild("selection").GetComponent<Renderer>();
            selection.enabled = false;
            infoJoueur = GameObject.Find("Canvas").transform.FindChild("InfoJoueur").GetComponent<InfoJoueurController>();
            flecheController = transform.FindChild("fleche").GetComponent<FlecheController>();

            view = GetComponent<PhotonView>();

            // A set dans le instanciate (Main)
            //player = new Player(5, 5, 15, 5, gameObject.name, team_id); 
            //isMine = true;

            arrivalPointPasse = new Vector3(0, 0, 0);
            arrivalPoint = new Vector3(0, 0, 0);

            myCollider = GetComponent<Collider>();
            limiteTerrain = GameObject.Find("terrain").transform.FindChild("limites").gameObject;

            anim = transform.FindChild("perso").GetComponent<Animator>();
            animationDeplacement = "Course";
            phaseAnimation = false;
            movement = false;
        }
        void Update()
        {
			if (Input.GetKeyDown(KeyCode.R))
			{
				Caller.SuccessAttack (this.player);
			}
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

        public void updateValuesPlayer(Color c) //Activation clic boutton
        {
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

        public void start_Anim(bool setPoint = true) // debut de l'animation
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
                arrivalPoint = new Vector3(menuController.Get_Coordsdeplacement[0], transform.position.y, menuController.Get_Coordsdeplacement[1]); // point d'arrivé du déplacement
                arrivalPointPasse = new Vector3(menuController.Get_CoordsPasse[0], 0.2f, menuController.Get_CoordsPasse[1]); // point d'arrivé de la passe
            }

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

		public override void OnSucceedAttack(Player p){
			Debug.Log ("I have succeed my Attack : " + p.Name + " - " + name);
		}
		public override void OnSucceedEsquive(Player p){
			Debug.Log ("I Have succeed my Esquive : " + p.Name + " - " + name);
		}
		public override void OnFailedAttack(Player p){
			Debug.Log ("I Have failed my Esquive : " + p.Name + " - " + name);
		}
		public override void OnFailedEsquive(Player p){
			Debug.Log ("I Have failed my Esquive : " + p.Name + " - " + name);
		}

		public override void OnStartAnimation(){
			start_Anim ();
		}
		public override void OnStartReflexion(){
			end_Anim();
		}


        // -- Network
        public void SyncValues()
        {
            if (!isMine)
                return;

            var player_values = new Dictionary<string, object>();
            player_values.Add("PointDep", serialize_vector3(this.PointDeplacement));
            player_values.Add("PointPasse", serialize_vector3(this.PointPasse));
            player_values.Add("BtnValues", this.Player.Button_Values);

            view.RPC("GetMyParam", PhotonTargets.Others, view.viewID, (byte[])ObjectToByteArray(player_values));
            start_Anim(false);
        }

        [PunRPC]
        private void GetMyParam(int viewID, byte[] param)
        {
            if (viewID != view.viewID)
                return;
            Dictionary<string, object> values = (Dictionary<string, object>)ByteToObject(param);
            this.PointDeplacement = deserialize_vector3((float[])values["PointDep"]);
            this.PointPasse = deserialize_vector3((float[])(values["PointPasse"]));
            this.Player.Button_Values = (List<string>)(values["BtnValues"]);
            start_Anim(false);
        }

        #region Serialization
        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        private object ByteToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            object obj = (object)binForm.Deserialize(memStream);

            return obj;
        }
        private float[] serialize_vector3(Vector3 v)
        {
            float[] result = new float[3];
            result[0] = v.x;
            result[1] = v.y;
            result[2] = v.z;
            return result;
        }
        private Vector3 deserialize_vector3(float[] values)
        {
            return new Vector3(values[0], values[1], values[2]);
        }
        #endregion

    }
}