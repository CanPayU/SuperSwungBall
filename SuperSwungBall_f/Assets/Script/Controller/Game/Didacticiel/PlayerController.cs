using UnityEngine;
using System.Collections.Generic;

using GameKit;

namespace GameScene.Didacticiel
{
    public class PlayerController : BasicPlayerController
    {
        public bool settablePointDeplacement = false;

        public PlayerController()
        {
            this.eventType = GameKit.EventType.All;
        }
        public MenuController menucontroller
        {
            get { return menuController; }
        }

        protected override void Start()
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

            Debug.Log("start");
            myCollider = GetComponent<Collider>();
            limiteTerrain = GameObject.Find("terrain").transform.FindChild("limites").gameObject;

            anim = transform.FindChild("perso").GetComponent<Animator>();
            animationDeplacement = "Course";
            phaseAnimation = false;
            movement = false;
        }

        //maintenant directement géré dans le basicPlayerController
        //public override void updateValuesPlayer(Color c) //Activation clic boutton
        //{
        //    menucontroller.update_Color(c); //change la couleur des trois bouttons "valeur"
        //    player.updateValues(convertColorToValue(c)); // Change les Stats du player 
        //    flecheController.changeColor(c); // change les couleurs de la flèche de déplacement
        //    menuController.update_zoneDeplacement(player.ZoneDeplacement, player.ZonePasse); // Change la tailles des zones
        //    flecheController.point(new Vector2(menuController.Get_Coordsdeplacement[0], menuController.Get_Coordsdeplacement[1])); //bouge la flèche de déplacement
        //}
        //private string convertColorToValue(Color c) //revoit la chaine de caractère liée à la couleur
        //{
        //    List<Color> colors = menuController.GetButtonsColor;
        //    if (c == colors[0])
        //        return "esquive";
        //    if (c == colors[1])
        //        return "tacle";
        //    if (c == colors[2])
        //        return "passe";
        //    return "course";
        //}
    }
}