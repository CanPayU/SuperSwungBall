using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace GameScene.Didacticiel
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private Text screentext;
        private GameObject player1_prefab;
        private GameObject player2_prefab;


        CameraController cameraController;
        InfoJoueurController infoJoueur;
        Collider thisCollider; //collider de ce game object
        Renderer[] flechesRenderer = new Renderer[4]; //renderer de ce game object
        Renderer ballRenderer; //renderer de la balle 
        Timer time;

        private string[,] tableau_1;
        private string[,] tableau_2;
        private string[,] tableau_3;
        private string[,] tableau_4;
        private float current_time;
        private int place;
        private int phase;

        private bool annim_started = false;
        private BasicPlayerController player_phase_2;

        // Use this for initialization
        void Start()
        {
            this.player1_prefab = Resources.Load("Prefabs/Solo/Player_1") as GameObject;
            this.player2_prefab = Resources.Load("Prefabs/Solo/Player_2") as GameObject;
            // -- Renderers / Collider
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
                {"","0" }};

            tableau_3 = new string[,] {
                {"Bien, maintenant nous allons voir comment récupérer la balle", "1"},
             //   {"Pour récupérer la balle et faire une passe, \n il faut au moins un point de passe", "1"},
             //   {"Met 1 point de maîtrise dans la passe et \n 2 points dans la course", "1"},
                {"","0"}};

            tableau_4 = new string[,] {
                {"Nous avons désormais la balle, envoyons-la quelque part", "1" },
             //   {"Pour envoyer la balle,\n il faut au moins un point de maîtrise de passe", "1"},
             //   {"Met 3 ponts de maîtrise dans la passe pour faire un passe très loin", "1"},
             //   {"Clique sur la zone rose, puis déplace le curseur \n pour marquer la direction de la passe", "1" },
                { "", "0" } };
            // --
        }
        // Update is called once per frame
        void Update()
        {
            switch (phase)
            {
                case 1:
                    phase1();
                    break;
                case 2:
                    phase2();
                    break;
                case 3:
                    phase3();
                    break;
                case 4:
                    phase4();
                    break;
                case 5:
                    phase5();
                    break;
                case 7: //6 = collision main
                    phase7();
                    break;
                case 8:
                    phase8();
                    break;
                case 10: //9 = collision balle
                    phase10();
                    break;
                case 11:
                    phase11();
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
            //Team team_0 = Game.Instance.Teams[0];
            Player play_t0 = Settings.Instance.Default_player["gpdn"];
            GameObject play0 = Instantiate(player1_prefab, new Vector3(1F, 1F, 0F), Quaternion.identity) as GameObject;
            play_t0.Team_id = 0;
            play_t0.Name += "";
            play0.name = play_t0.Name + "-" + play_t0.Team_id;
            this.player_phase_2 = (BasicPlayerController)play0.AddComponent(typeof(GameScene.Solo.PlayerController));
            this.player_phase_2.Player = play_t0;
            this.player_phase_2.IsMine = true;
            phase++;
            Debug.Log(play0.name);
        }
        void phase3()
        {
            screentext.transform.position = new Vector2(930, 700);
            phase++;
        }
        void phase4()
        {
            text(tableau_2);
        }
        void phase5()
        {
            screentext.text = "Déplace le Swungman jusqu'ici \n et appuie sur 'Espace' pour lancer le déplacement";
            foreach (Renderer r in flechesRenderer)
                r.enabled = true;
            phase++;
        }
        void phase7()
        {
            text(tableau_3);
        }
        void phase8()
        {
            screentext.text = "Déplace toi maintenant vers la balle, ('Espace' pour lancer le déplacement)";
            ballRenderer.enabled = true;
            this.transform.position = new Vector3(5, 0);
            foreach (Renderer r in flechesRenderer)
                r.enabled = true; //remplacer par false
            phase++;
        }
        void phase10()
        {
            text(tableau_4);
        }
        void phase11()
        {
            foreach (Renderer r in flechesRenderer)
                r.enabled = true;
        }
        // --

        // -- Text
        void text(string[,] tableau)
        {
            float temps = float.Parse(tableau[place, 1]);

            if (place == 0) //premier passage
            {
                screentext.text = message(tableau);
            }

            if (temps == 0) //dernier passage
            {
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
            if (objet.name == "GPasDNom-0") //collision avec un Player ?
            {
                if (phase == 6)
                {
                    phase++;
                    foreach (Renderer r in flechesRenderer)
                        r.enabled = true; //a changer en false
                    this.transform.position = new Vector3(5, 0);
                    end_time();
                }
                if (phase == 9)
                {
                    phase++;
                    this.transform.position = new Vector3(0, 0);
                    end_time();
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
                this.player_phase_2.end_Anim();
                annim_started = false;
                thisCollider.enabled = false; //désactiver le collider quand on est pas en phase d'animation
            }
        }
        private void start_annim()
        {
            thisCollider.enabled = true; //réactiver le collider quand on est en phase d'animation
            annim_started = true;
            time.start();
            this.player_phase_2.start_Anim();
        }
        // --

    }//Class
}//Namespace
