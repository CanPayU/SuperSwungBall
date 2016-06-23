using GameKit;
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

        [SerializeField]
        private Text screentext;

        private GameObject player1_prefab;
        private GameObject player2_prefab;

        GameObject myPlayer; //le player
        GameObject BouttonEsquive;
        GameObject BouttonTacle;
        GameObject BouttonPasse;
        GameObject BouttonCourse;
        private PlayerController MyPlayer_Controller;

        GameObject myPlayer2; //le deuxième player
        private PlayerController MyPlayer2_Controller;

        GameObject enemyPlayer; //ne peut être controllé par le joueur
        private PlayerController EnemyPlayer_Controller;


        GameObject ball;
        Renderer ballRenderer; //renderer de la balle 

        //CameraController cameraController;
        //InfoJoueurController infoJoueur;
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
        private string[,] tableau_7;
        private string[,] tableau_8;
        private string[,] tableau_9;
        private float current_time;
        private int place;
        private int phase;

        private bool annim_started = false;

        Color cCourse;
        Color cTacle;
        Color cEsquive;
        // Use this for initialization
        void Start()
        {
            this.player1_prefab = Resources.Load("Prefabs/Didacticiel/Player_1") as GameObject;
            this.player2_prefab = Resources.Load("Prefabs/Didacticiel/Player_2") as GameObject;

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
            //cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
            time = new Timer(10.0F, end_time);
            // --


            // -- text
            #region text
            phase = 1;
            place = 0;
            current_time = 0;
            //mettre "0" en temps pour que le message reste à l'écran

            tableau_1 = new string[,]
            {
                {"Bienvenue dans le didacticiel", "1" },
                {"Comment jouer à SuperSwungBall ?", "1" },
                {"Le but du jeu est de marquer 3 points", "1" },
                {"Pour cela, chaque joueur contrôle les joueurs de son équipe", "1" },
                {"Commençons par voir les contrôles \n d'un Swungman", "1" }
            };

            tableau_2 = new string[,]
            {
                {"Ça , c'est un Swungman, \n appuie dessus pour le contrôler", "1.5" },
                {"Les capacités de déplacements sont représentées \n par la couleur bleu", "2" },
                {"Appuie sur le bouton bleu 3 fois pour le faire \n courir le plus vite et le plus loin possible", "2" },
                {"Clique sur la zone de déplacement (bleue) pour contrôler \n le déplacement","2" },
                {"Déplace le Swungman jusqu'ici \n et appuie sur 'Espace' pour lancer le déplacement", "0" }
            };

            tableau_3 = new string[,]
            {
                {"Bien, maintenant nous allons voir comment récupérer la balle", "1"} ,
                {"Pour récupérer la balle et faire une passe, \n il faut au moins un point de maîtrise de la balle", "2" },
                {"Mets 1 point de maîtrise dans la passe (en rose) \n et 2 points dans la course", "1.5" },
                {"Déplace toi maintenant vers la balle, \n ('Espace' pour lancer le déplacement)", "0" } }
            ;

            tableau_4 = new string[,]
            {
                {"Nous avons désormais la balle, envoyons-la quelque part", "1.5" },
                {"Pour envoyer la balle,\n il faut au moins un point de maîtrise de passe", "1.5" },
                {"Mets 3 ponts de maîtrise dans la passe pour faire une passe très loin", "1.5" },
                {"Clique sur l'extrémité de la zone rose, puis déplace le point rose \n pour marquer l'emplacement de la passe", "2.5" },
                {"Tu peux désormais lancer l'action \n et appuyer sur 'a' pour faire la passe", "0" }
            };

            tableau_5 = new string[,]
            {
                {"Quelle passe ! Mais attention à toi, \n un ennemi arrive","1" },
                {"Il a récupéré la balle et veut continuer son chemin. \n Nous devons l'en empêcher","2" },
                {"Pour engager un combat il n'y a qu'une seul moyen : \n il faut qu'au moins un des deux joueurs possède du tacle","2.5" },
                {"Il n'a sûrement pas envie de se battre puisqu'il a la balle, \n c'est donc a nous d'engager le combat !","2.5" },
                {"Mets deux points dans le tacle (vert) et un point dans la passe \n (pour récupérer la balle si tu le tacles) et fonce vers lui","0" }
            };

            tableau_6 = new string[,]
            {
                {"En voilà un bon tacle ! Tu y es allé tellement fort \n qu'il a fini à terre et n'a pas terminé son déplacement","2.5" },
                {"En temps normal quand un ennemi finit à terre \n il est affaiblit pour le prochain tour","2.5" },
                {"Je le soigne donc maintenant pour le bien de notre entraînement. \n La belle vie c'est pour plus tard","2.5" },
                {"Mais désormais ça va être à toi de te défendre \n car tu as récupéré la balle","2" },
                {"Il va sûrement essayer de te tacler pour te faire perdre la balle","1.5" },
                {"Mets donc un maximum de points dans ta capacité d'esquive (jaune) \n et fonce vers lui","0" }
            };

            tableau_7 = new string[,]
            {
                {"Comme prévu il a engagé le combat et nous avons gagné \n car nous avions plus de \"force de combat\" que lui ","2.5" },
                {"La force de combat c'est la valeur maximale mise dans le tacle OU l'esquive \n il faut donc bien faire attention à bien agencer ces points","3" },
                {"Chaque personnage a des stats de base différentes dans chaque maîtrise","2" },
                {"Bien connaître ces stats est donc primordiale \n pour ne pas être surpris de l'issu d'un combat","2.5" },
                {"Fait quand même attention à ne pas trop abuser des combats \n car plus tu gagnes un combat, plus tu seras affaibli pour le prochain","3" },
                {"Faire la passe à un coéquipier, même s'il est derrière nous, \n peux ainsi être un bon moyen de passer la défense ennemi","2.5" },
            };

            tableau_8 = new string[,]
            {
                {"Tu as désormais toutes les armes en main \n pour te confronter aux autres joueurs !","2.5" },
                {"Pour prouver que tu es vraiment à la hauteur, \n tu dois passer une ultime épreuve","2.5" },
                {"Pour finir nous allons marquer un but à l'aide d'un coéquipier","2" },
                {"Tu es libre de marquer le but comme tu veux \n c'est l'heure de mettre en pratique tout ce que tu as appris","0" }
            };

            tableau_9 = new string[,]
            {
                {"Super but !","1.5" },
                {"Il te reste beaucoup à apprendre pour devenir le meilleur \n mais je vois en toi un grand potentiel, tu vas réussir","2" },
                {"Il temps de montrer ce que tu vaux en affrontant des joueurs ! \n les choses sérieuses commencent enfin","2" },
                {"Bonne chance...","3" }
            };
            #endregion
            // --
        }
        // Update is called once per frame
        void Update()
        {
            switch (phase)
            {
                case 1:
                    text(tableau_1);
                    break;
                case 2:
                    phase2(); //création du player + player2(mais il n'est pas disponible pour le moment) & changement emplacement du texte & 
                    break;
                case 3:
                    text(tableau_2);
                    break;
                case 4:
                    phase4(); //flèches main + rond blanc visibles & player2 désactivé(impossible de le faire lors de la phase2)
                    break;
                //5 = collision main & activation maitrise de la balle
                case 6:
                    text(tableau_3);
                    break;
                case 7:
                    phase7(); //balle visile & flèches main + rond blanc non visibles
                    break;
                case 8:
                    phase8(); //récupération balle & désactivation boutton course
                    break;
                case 9:
                    text(tableau_4);
                    break;
                case 10:
                    phase10(); //affichage des 4 flèches + rond blanc
                    break;
                //11 = passe : collision balle & activation esquive + désactivation des autres & création joueur adverse & 3 courses pour le joueur
                case 12:
                    text(tableau_5);
                    break;
                //13 = déplacement joueur adverse & tacle par le player
                case 14:
                    phase14(); //3 courses pour le joueur & seulement esquive disponible
                    break;
                case 15:
                    text(tableau_6);
                    break;
                //16 = déplacement joueur adverse & esquive réussie par notre joueur
                case 17:
                    if (!annim_started)
                        text(tableau_7);
                    break;
                case 18:
                    phase18(); //tout les bouttons sont visibles pour le player & le player2 est là & changement de position player + adversaire
                    break;
                case 19:
                    text(tableau_8);
                    break;
                //20 = marquer un but
                case 21:
                    text(tableau_9);
                    break;
                case 22:
                    FadingManager.Instance.Fade();
                    break;
            }
            time.update();
            if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
            {
                start_annim();
            }
        }

        // -- Phases
        void phase2()
        {
            //le premier player
            Player play_t0 = Settings.Instance.Default_player["itec"];
            myPlayer = Instantiate(player1_prefab, new Vector3(1F, 1F, 0F), Quaternion.identity) as GameObject;
            play_t0.Team_id = 0;
            play_t0.Name = "#Test</§!";
            myPlayer.name = play_t0.Name + "-" + play_t0.Team_id;
            MyPlayer_Controller = (PlayerController)myPlayer.AddComponent(typeof(PlayerController));
            MyPlayer_Controller.Player = play_t0;
            MyPlayer_Controller.IsMine = true;
            //le deuxième player
            Player play_t2 = Settings.Instance.Default_player["lombrix"];
            myPlayer2 = Instantiate(player1_prefab, new Vector3(40F, 1F, 40F), Quaternion.identity) as GameObject; //trop loin pour être vu
            play_t2.Team_id = 0;
            play_t2.Name = "G1aD0s";
            myPlayer2.name = play_t2.Name + "-" + play_t2.Team_id;
            MyPlayer2_Controller = (PlayerController)myPlayer2.AddComponent(typeof(PlayerController));
            MyPlayer2_Controller.Player = play_t2;
            MyPlayer2_Controller.IsMine = true;


            screentext.transform.position = new Vector2(960, 700);
            phase++;
        }
        void phase4()
        {

            BouttonEsquive = myPlayer.transform.FindChild("menu(Clone)").FindChild("boutton1").gameObject;
            BouttonTacle = myPlayer.transform.FindChild("menu(Clone)").FindChild("boutton2").gameObject;
            BouttonPasse = myPlayer.transform.FindChild("menu(Clone)").FindChild("boutton3").gameObject;
            BouttonCourse = myPlayer.transform.FindChild("menu(Clone)").FindChild("boutton4").gameObject;

            cEsquive = MyPlayer_Controller.menucontroller.GetButtonsColor[0]; //couleur de l'esquive
            cTacle = MyPlayer_Controller.menucontroller.GetButtonsColor[1];   //couleur du tacle
            cCourse = MyPlayer_Controller.menucontroller.GetButtonsColor[3];  //couleur de la passe

            BouttonEsquive.SetActive(false);
            BouttonPasse.SetActive(false);
            BouttonTacle.SetActive(false);

            foreach (Renderer r in flechesRenderer)
                r.enabled = true;
            RondBlanc.enabled = true;
            phase++;
        }
        void phase7()
        {
            ballRenderer.enabled = true;
            this.transform.position = new Vector3(5, 0);
            foreach (Renderer r in flechesRenderer)
                r.enabled = false;
            RondBlanc.enabled = false;
            phase++;
        }
        void phase8()
        {
            if (ball.transform.IsChildOf(myPlayer.transform))
            {
                end_time();
                this.transform.position = new Vector3(2, 0);
                BouttonCourse.SetActive(false);
                phase++;
            }
        }
        void phase10()
        {
            foreach (Renderer r in flechesRenderer)
                r.enabled = true;
            RondBlanc.enabled = true;
            phase++;
        }
        void phase14()
        {
            if (!annim_started)
            {
                BouttonEsquive.SetActive(true);
                BouttonTacle.SetActive(false);
                BouttonPasse.SetActive(false);

                MyPlayer_Controller.updateValuesPlayer(cCourse);
                MyPlayer_Controller.updateValuesPlayer(cCourse);
                MyPlayer_Controller.updateValuesPlayer(cCourse);
                phase++;
            }
        }
        void phase18()
        {
            BouttonCourse.SetActive(true);
            BouttonPasse.SetActive(true);
            BouttonTacle.SetActive(true);
            myPlayer.transform.position = new Vector3(1, 1, 21);
            myPlayer2.transform.position = new Vector3(5, 1, 17);
            enemyPlayer.transform.position = new Vector3(5, 1, 24);

            MyPlayer_Controller.Player.BallHolder = false;
            ball.transform.position = myPlayer2.transform.position;
            ball.transform.parent = myPlayer2.transform.FindChild("perso");
            MyPlayer2_Controller.Player.BallHolder = true;

            phase++;
        }
        // --


        // -- Text
        void text(string[,] tableau)
        {
            float temps = float.Parse(tableau[place, 1]);

            if (place == 0 && current_time == 0) // premier passage ? -> changer direct le texte
                message(tableau);

            if (temps == 0) // message qui reste à l'écran ? laisser le texte à l'écran et passer à la phase suivante
            {
                message(tableau);
                phase++;
                place = 0;
                current_time = 0;
            }
            else if (current_time < temps) // temps pas atteint ? -> continuer à afficher
                current_time += Time.deltaTime;
            else // temps atteint ? -> réinitialiser le temps et voir si on continue de lire le tableau
            {
                current_time = 0;
                if (tableau.GetLongLength(0) == place + 1) // dernier passage-> réinitialiser le texte et passer à la phase suivante
                {
                    screentext.text = "";
                    phase++;
                    place = 0;
                }
                else // sinon passer au message suivant
                {
                    place += 1;
                    message(tableau);
                }
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
            if (objet == myPlayer) //collision avec le Player ?
            {
                if (phase == 5) //premier déplacement
                {
                    end_time();
                    foreach (Renderer r in flechesRenderer)
                        r.enabled = false;
                    RondBlanc.enabled = false;
                    BouttonPasse.SetActive(true);
                    this.transform.position = new Vector3(6, 0, 0);
                    phase++;
                }
            }
            if (objet == ball) //collision avec la balle ?
            {
                if (phase == 11) //lancer la balle première fois
                {
                    end_time();
                    //main
                    this.transform.position = new Vector3(0, 0, 5);
                    foreach (Renderer r in flechesRenderer)
                        r.enabled = false;
                    RondBlanc.enabled = false;
                    //myPlayer
                    BouttonTacle.SetActive(true);
                    BouttonCourse.SetActive(false);

                    MyPlayer_Controller.updateValuesPlayer(cCourse);
                    MyPlayer_Controller.updateValuesPlayer(cCourse);
                    MyPlayer_Controller.updateValuesPlayer(cCourse);
                    //enemyPlayer
                    Player play_t1 = Settings.Instance.Default_player["lombrix"];
                    enemyPlayer = Instantiate(player2_prefab, new Vector3(ball.transform.position.x, 1F, ball.transform.position.z), Quaternion.identity) as GameObject;
                    play_t1.Team_id = 1;
                    enemyPlayer.name = play_t1.Name + "-" + play_t1.Team_id;
                    EnemyPlayer_Controller = (PlayerController)enemyPlayer.AddComponent(typeof(PlayerController));
                    EnemyPlayer_Controller.Player = play_t1;
                    EnemyPlayer_Controller.IsMine = false;
                    EnemyPlayer_Controller.settablePointDeplacement = true;

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
                {
                    Debug.Log("ok c'est bien finit pour l'ennemi et la phase est : " + phase);
                    this.EnemyPlayer_Controller.end_Anim();
                }
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
            if (phase == 20)
                MyPlayer2_Controller.start_Anim();

            if (EnemyPlayer_Controller != null)
            {
                EnemyPlayer_Controller.PointDeplacement = new Vector3( //il se dirige vers le joueur #LaMeilleurIA
                    myPlayer.transform.position.x,
                    myPlayer.transform.position.y,
                    myPlayer.transform.position.z);
                Debug.Log("start anim ennemi phase : " + phase);
                if (phase == 13)
                {
                    ball.transform.position = enemyPlayer.transform.position;
                    ball.transform.parent = enemyPlayer.transform.FindChild("perso");
                    MyPlayer2_Controller.Player.BallHolder = true;
                    EnemyPlayer_Controller.updateValuesPlayer(cCourse); //course / course / course
                    EnemyPlayer_Controller.updateValuesPlayer(cCourse);
                    EnemyPlayer_Controller.updateValuesPlayer(cCourse);
                }
                if (phase == 16)
                {
                    EnemyPlayer_Controller.updateValuesPlayer(cTacle);  //tacle / course /course
                    EnemyPlayer_Controller.updateValuesPlayer(cCourse);
                    EnemyPlayer_Controller.updateValuesPlayer(cCourse);
                }
                if (phase == 20)
                {
                    EnemyPlayer_Controller.PointDeplacement = new Vector3( //il se dirige vers le joueur2 #LaMeilleurIA
                       myPlayer2.transform.position.x,
                       myPlayer2.transform.position.y,
                       myPlayer2.transform.position.z);

                    EnemyPlayer_Controller.updateValuesPlayer(cTacle);  //tacle / course / course
                    EnemyPlayer_Controller.updateValuesPlayer(cEsquive);
                    EnemyPlayer_Controller.updateValuesPlayer(cEsquive);
                }
                EnemyPlayer_Controller.start_Anim(false);               //animation du joueur ennemi
            }
        }
        public override void OnSucceedAttack(Player pl)
        {
            if (phase == 13)
                phase++;
        }
        public override void OnSucceedEsquive(Player p)
        {
            if (phase == 16)
                phase++;
        }

        public override void OnGoal(GoalController goal)
        {
            //inutile ?
            Debug.Log("on goal enter");
        }

        public void update_score()
        {
            Debug.Log("update score");
            if (phase == 20)
                phase++;
        }
        // --

    }//Class
}//Namespace
