﻿using GameKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene.Didacticiel
{
    public class MainController : GameBehavior
    {
        public MainController()
        {
            this.eventType = GameKit.EventType.External;
        }
        public override void OnSucceedEsquive(Player p)
        {
            phase++;
            //InstanciateMessage(p.Name + " réussit son esquive !", ChatController.Chat.EVENT);
        }

        [SerializeField]
        private Text screentext;
        [SerializeField]
        private GameObject player1_prefab;
        [SerializeField]
        private GameObject player2_prefab;

        GameObject play0; //le player
        GameObject BouttonEsquive;
        GameObject BouttonTacle;
        GameObject BouttonPasse;
        GameObject BouttonCourse;
        private BasicPlayerController MyPlayer_Controller;

        GameObject enemyPlayer; //ne peut être controllé par le joueur
        private BasicPlayerController EnemyPlayer_Controller;

        GameObject ball;
        Renderer ballRenderer; //renderer de la balle 

        CameraController cameraController;
        InfoJoueurController infoJoueur;
        Collider thisCollider; //collider de ce game object
        Renderer[] flechesRenderer = new Renderer[4]; //renderer des 4 flèches de ce game object
        Renderer RondBlanc; //rond blanc sur ce game object
        Timer time;

        private string[,] tableau_1;
        private string[,] tableau_2;
        private string[,] tableau_3;
        private string[,] tableau_4;
        private string[,] tableau_5;
        private string[,] tableau_6;
        private float current_time;
        private int place;
        private int phase;

        private bool annim_started = false;

        // Use this for initialization
        void Start()
        {
            this.player1_prefab = Resources.Load("Prefabs/Solo/Player_1") as GameObject;
            this.player2_prefab = Resources.Load("Prefabs/Solo/Player_2") as GameObject;

            // -- Renderers / Collider
            RondBlanc = this.transform.FindChild("Plane").GetComponent<Renderer>();
            RondBlanc.enabled = false;

            for (int i = 0; i < 4; i++)
            {
                flechesRenderer[i] = transform.FindChild("fleche" + (i + 1)).FindChild("flecheTuto").GetComponent<Renderer>();
            }
            foreach (Renderer r in flechesRenderer)
            {
                r.material.SetColor("_Color", Color.cyan);
                r.enabled = false;
            }

            thisCollider = this.GetComponent<Collider>();

            ball = GameObject.Find("Ball");
            ballRenderer = GameObject.Find("Ball").GetComponent<Renderer>();
            ballRenderer.enabled = false;
            // --


            // --
            cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
            time = new Timer(10.0F, end_time);
            // --


            // -- text
            phase = 1;
            place = 0;
            current_time = 0;
            tableau_1 = new string[,] {
               {"Bienvenue dans le didacticiel", "1"},
           //     {"Comment jouer ?", "1"},
           //     {"Le but du jeu est de marquer 3 points", "1"},
           //     {"Chaque joueur contrôle son équipe", "1"},
           //     {"Commençons par voir les contrôles \n d'un Swungman", "1"},
                {"", "0" }};

            tableau_2 = new string[,] {
                {"Ca c'est un Swungman, \n appuie dessus pour pouvoir le contrôler", "1" },
            //    {"Les capacités de déplacements sont représentées \n par la couleur bleu", "1"},
            //    {"Appuie sur le bouton bleu 3 fois pour le faire \n courire le plus vite et le plus loin possible", "1"},
            //    {"Clique sur la zone de déplacement (bleue) pour contoller \n le déplacement","1" },
                {"Déplace le Swungman jusqu'ici \n et appuie sur 'Espace' pour lancer le déplacement", "0" } };

            tableau_3 = new string[,] {
                {"Bien, maintenant nous allons voir comment récupérer la balle", "1"},
             //   {"Pour récupérer la balle et faire une passe, \n il faut au moins un point de passe", "1"},
             //   {"Met 1 point de maîtrise dans la passe et \n 2 points dans la course", "1"},
                {"Déplace toi maintenant vers la balle, ('Espace' pour lancer le déplacement)", "0" } };

            tableau_4 = new string[,] {
                {"Nous avons désormais la balle, envoyons-la quelque part", "1" },
             //   {"Pour envoyer la balle,\n il faut au moins un point de maîtrise de passe", "1"},
             //   {"Met 3 ponts de maîtrise dans la passe pour faire un passe très loin", "1"},
             //   {"Clique sur la zone rose, puis déplace le point rose \n pour marquer l'emplacement de la passe", "1" },
                {"", "0" } };

            tableau_5 = new string[,] {
                {"Quelle passe ! Mais attention à toi, \n un ennemi arrive","1"},
              //  {"Il a sûrement envie de te tacler, \n même si tu n'as pas la balle","1" },
                {"Met donc un maximum de point dans ta capacité d'esquive \n et attend le ici (espace pour lancer l'action)","0" } };
            // --
        }
        // Update is called once per frame
        void Update()
        {
            switch (phase)
            {
                case 1:
                    phase1(); //affichage premier texte
                    break;
                case 2:
                    phase2(); //création du player & changement emplacement du texte
                    break;
                case 3:
                    phase3(); //affichage deuxième texte
                    break;
                case 4:
                    phase4(); //flèches main + rond blanc visibles
                    break; //5 = collision main & activation maitrise de la balle
                case 6:
                    phase6(); //affichage troisième tableau
                    break;
                case 7:
                    phase7(); //balle visile & flèches main + rond blanc non visibles
                    break;
                case 8:
                    phase8(); //récupération balle
                    break;
                case 9:
                    phase9(); //affichage quatrième tableau
                    break;
                case 10:
                    phase10(); //affichage des 4 flèches + rond blanc
                    break;//11 = passe : collision balle & activation esquive
                case 12:
                    phase12();
                    break;
            }
            time.update();
            if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
            {
                start_annim();
            }
        }

        // -- Phases
        void phase1()
        {
            text(tableau_1);
        }
        void phase2()
        {
            Player play_t0 = Settings.Instance.Default_player["gpdn"];
            play0 = Instantiate(player1_prefab, new Vector3(1F, 1F, 0F), Quaternion.identity) as GameObject;
            play_t0.Team_id = 0;
            play_t0.Name += "";
            play0.name = play_t0.Name + "-" + play_t0.Team_id;
            this.MyPlayer_Controller = (BasicPlayerController)play0.AddComponent(typeof(GameScene.Solo.PlayerController));
            this.MyPlayer_Controller.Player = play_t0;
            this.MyPlayer_Controller.IsMine = true;

            screentext.transform.position = new Vector2(960, 700);
            phase++;
        }
        void phase3()
        {
            text(tableau_2);
        }
        void phase4()
        {
            BouttonEsquive = play0.transform.FindChild("menu(Clone)").FindChild("boutton1").gameObject;
            BouttonTacle = play0.transform.FindChild("menu(Clone)").FindChild("boutton2").gameObject;
            BouttonPasse = play0.transform.FindChild("menu(Clone)").FindChild("boutton3").gameObject;
            BouttonCourse = play0.transform.FindChild("menu(Clone)").FindChild("boutton4").gameObject;

            BouttonEsquive.SetActive(false);
            BouttonPasse.SetActive(false);
            BouttonTacle.SetActive(false);

            foreach (Renderer r in flechesRenderer)
                r.enabled = true;
            RondBlanc.enabled = true;
            phase++;
        }
        void phase6()
        {
            text(tableau_3);
        }
        void phase7()
        {
            //screentext.text = "Déplace toi maintenant vers la balle, ('Espace' pour lancer le déplacement)";
            ballRenderer.enabled = true;
            this.transform.position = new Vector3(5, 0);
            foreach (Renderer r in flechesRenderer)
                r.enabled = false;
            RondBlanc.enabled = false;
            phase++;
        }
        void phase8()
        {
            if (ball.transform.IsChildOf(play0.transform))
            {
                this.transform.position = new Vector3(0, 0);
                end_time();
                phase++;
            }
        }
        void phase9()
        {
            text(tableau_4);
        }
        void phase10()
        {
            foreach (Renderer r in flechesRenderer)
                r.enabled = true;
            RondBlanc.enabled = true;
            phase++;
        }
        void phase12()
        {
            text(tableau_5);
        }
        // --


        // -- Text
        void text(string[,] tableau)
        {
            float temps = float.Parse(tableau[place, 1]);

            if (place == 0) //premier passage
                screentext.text = message(tableau);

            if (temps == 0) //dernier passage
            {
                screentext.text = message(tableau);
                phase++;
                current_time = 0;
                place = 0;
            }
            else if (current_time < temps) // temps pas atteint ? -> continuer à afficher
                current_time += Time.deltaTime;
            else // temps atteint ? -> changer de texte
            {
                current_time = 0;
                place += 1;
            }
        }
        string message(string[,] tableau)
        {
            return screentext.text = tableau[place, 0];
        }
        // --

        // Collisions
        void OnTriggerEnter(Collider other)
        {
            GameObject objet = other.gameObject;
            if (objet.name == "GPasDNom-0") //collision avec le Player ?
            {
                if (phase == 5) //premier déplacement
                {
                    foreach (Renderer r in flechesRenderer)
                        r.enabled = false;
                    RondBlanc.enabled = false;
                    BouttonPasse.SetActive(true);
                    this.transform.position = new Vector3(6, 0, 0);
                    end_time();
                    phase++;
                }
                /*
                if (phase == 8) //récupérer la balle première fois
                {
                    this.transform.position = new Vector3(0, 0);
                    end_time();
                    phase++;
                }
                */
            }
            if (objet.name == "Ball") //collision avec la balle ?
            {
                if (phase == 11) //lancer la balle première fois
                {
                    this.transform.position = new Vector3(0, 0, 5);
                    foreach (Renderer r in flechesRenderer)
                        r.enabled = false;
                    RondBlanc.enabled = false;

                    BouttonEsquive.SetActive(true);
                    BouttonPasse.SetActive(false);
                    BouttonCourse.SetActive(false);

                    Player play_t1 = Settings.Instance.Default_player["epta"];
                    enemyPlayer = Instantiate(player2_prefab, new Vector3(2F, 1F, 2F), Quaternion.identity) as GameObject;
                    play_t1.Team_id = 1;
                    play_t1.Name += "";
                    enemyPlayer.name = play_t1.Name + "-" + play_t1.Team_id;
                    this.EnemyPlayer_Controller = (BasicPlayerController)enemyPlayer.AddComponent(typeof(GameScene.Solo.PlayerController));
                    this.EnemyPlayer_Controller.Player = play_t1;
                    this.EnemyPlayer_Controller.IsMine = false;

                    EnemyPlayer_Controller.PointDeplacement = new Vector3(3F, 1F, 3F);

                    end_time();
                    phase++;
                }
            }
        }
        // --

        // -- Anim'
        private void end_time()
        {
            time.reset();
            if (annim_started)
            {
                MyPlayer_Controller.end_Anim();
                if (EnemyPlayer_Controller != null)
                    this.EnemyPlayer_Controller.end_Anim();
                annim_started = false;
                thisCollider.enabled = false; //désactiver le collider quand on est pas en phase d'animation
            }
        }
        private void start_annim()
        {
            thisCollider.enabled = true; //réactiver le collider quand on est en phase d'animation
            annim_started = true;
            time.start();
            MyPlayer_Controller.start_Anim();
            if (EnemyPlayer_Controller != null)
                EnemyPlayer_Controller.start_Anim();
        }
        // --

    }//Class
}//Namespace