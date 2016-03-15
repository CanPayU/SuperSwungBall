using UnityEngine;
using System.Collections.Generic;

namespace GameScene.Multi
{
    public class PlayerController : MonoBehaviour
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

        //deplacement
        private bool deplacement;
        private float speed = 0;
        private Vector3 arrivalPoint;
        private bool movement;
        private float pause = 0; // temps de pause du deplacement (en s), quand le player se fait plaquer par exemple

        //Pointeur
        private bool mouseState = false;

        //passe
        private Vector3 arrivalPointPasse;

		//Network
		private PhotonView view;

		public bool Deplacement // = phase d'annim en solo
		{ get { return deplacement; } }
        public float Pause
        { set { pause = value; } }
        public Player Player 
        { get {  return player; } }

        void Start()
        {
            //initialisation menu
			Menu = Instantiate(Menu, new Vector3(), Quaternion.Euler(0,-90,0)) as GameObject;
            Menu.transform.parent = transform;
			menuController = Menu.GetComponent<MenuController>();

			view = GetComponent<PhotonView> ();

			player = new Player(5, 5, 15, 5, gameObject.name, team_id); // A changer en fonction des stats initiales du perso

			Team t_ = Game.Instance.Teams [PhotonNetwork.player.ID]; // team_id
			t_.add_player (player);
            myCollider = GetComponent<Collider>();

			deplacement = false;
            movement = false;
        }
        void Update()
        {
			if (!view.isMine)
				return;
			

            if (!deplacement)
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
							playAnnimation("Repos");
                            movement = false;
                        }
                    }
                }
                else
                {
                    pause -= Time.deltaTime;
                    if(pause < 0) // pause terminée
                    {
                        pause = 0;
                        if(movement) // play precedente animation
                        {
                            transform.FindChild("perso").LookAt(new Vector3(arrivalPoint.x, transform.FindChild("perso").position.y, arrivalPoint.z));
							playAnnimation("Course");
                        }
                        else
                        {
							playAnnimation("Repos");
                        }
                    }
                }
            }
        }
        /*
        public void OnTriggerEnter(Collider other) //event collison
        {
            if (deplacement)
            {
				Debug.Log ("Triger enter :" + other.name);
				if (other.transform.parent == null && other.name == "Ball" && player.ZonePasse != 0 && other.GetComponent<BallController>().interceptable(gameObject)) // ramasse/intercepte la balle uniquement si le perso a au moins un élément "passe"
				{
					Debug.Log ("Send request for :" + other.name);
					PhotonView ph = other.gameObject.GetComponent <PhotonView> ();
					ph.RequestOwnership ();


					PhotonView pv = PhotonView.Get (this);
					pv.RPC ("attached_ball", PhotonTargets.All, ph.viewID);
                }
            }
        }
		[PunRPC] private void attached_ball(int viewID){
			GameObject other = PhotonView.Find (viewID).gameObject;
			other.transform.parent = transform;
			other.transform.localPosition = new Vector3(1, 0, 0);
		}*/

        public void updateValuesPlayer(Color c) //Activation clic boutton
        {
            player.updateValues(convertColorToValue(c)); // Change les Stats du player 
            menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse); // Change la tailles des zones
        }
        private string convertColorToValue(Color c)
        {
            List<Color> colors = menuController.GetButtonsColor;
            if (c == colors[0])
                return "tacle";
            if (c == colors[1])
                return "esquive";
            if (c == colors[2])
                return "passe";
            return "course";
        }

        public bool passe(ref Vector3 pointPasse) // Passe
        {
            pointPasse = arrivalPointPasse;
            return (deplacement && Vector3.Distance(arrivalPointPasse, transform.position) < player.ZonePasse * 5);
        }

        public void start_Anim() // debut de l'animation
		{
            mouseState = false;
            deplacement = true;
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
				playAnnimation("Course");
                movement = true;
            }
        }
        public void end_Anim() // fin de l'animation
        {
            menuController.reset();
            player.reset();
            menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse);
            deplacement = false;
            speed = 0;
            pause = 0;

            //animation repos
			playAnnimation("Repos");
            movement = false;
        }

		/*
        public void OnTriggerEnter(Collider other) //event collison
        {
            if (deplacement)
            {
				Debug.Log ("Triger enter :" + other.name);
				if (other.transform.parent == null && other.name == "Ball" && player.ZonePasse != 0 && other.GetComponent<BallController>().interceptable(gameObject)) // ramasse/intercepte la balle uniquement si le perso a au moins un élément "passe"
				{
					Debug.Log ("Send request for :" + other.name);
					PhotonView ph = other.gameObject.GetComponent <PhotonView> ();
					ph.RequestOwnership ();


					PhotonView pv = PhotonView.Get (this);
					pv.RPC ("attached_ball", PhotonTargets.All, ph.viewID);
                }
            }
        }
		[PunRPC] private void attached_ball(int viewID){
			GameObject other = PhotonView.Find (viewID).gameObject;
			other.transform.parent = transform;
			other.transform.localPosition = new Vector3(1, 0, 0);
		}*/
		private void playAnnimation(string name){
			PhotonView ph = gameObject.GetComponent <PhotonView> (); // View sur laquelle on fait l'annim
			ph.RequestOwnership ();
			PhotonView pv = PhotonView.Get (this);
			pv.RPC ("playAnnimationRPC", PhotonTargets.All, ph.viewID, name); // sync
		}
		[PunRPC] private void playAnnimationRPC(int viewID, string name){
			GameObject other = PhotonView.Find (viewID).gameObject;
			other.transform.FindChild("perso").GetComponent<Animator>().Play(name);
		}

    }
}