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
    }
}