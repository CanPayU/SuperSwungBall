using GameKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TranslateKit;

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
        [SerializeField]
        private Text screetext2;

        private GameObject player1_prefab;
        private GameObject player2_prefab;

        Player play_t0; //le player
        GameObject myPlayer;
        GameObject BouttonEsquive;
        GameObject BouttonTacle;
        GameObject BouttonPasse;
        GameObject BouttonCourse;
        private PlayerController MyPlayer_Controller;

        Player play_t1; //le deuxieme player
        GameObject myPlayer2;
        private PlayerController MyPlayer2_Controller;

        Player play_t2;
        GameObject enemyPlayer; //ne peut etre controlle par le joueur
        private PlayerController EnemyPlayer_Controller;

        Player play_t3;
        GameObject enemyPlayer2; //ne peut etre controlle par le joueur
        Player play_t4;
        private PlayerController EnemyPlayer_Controller2;
        GameObject enemyPlayer3; //ne peut etre controlle par le joueur
        Player play_t5;
        private PlayerController EnemyPlayer_Controller3;
        GameObject enemyPlayer4; //ne peut etre controlle par le joueur
        Player play_t6;
        private PlayerController EnemyPlayer_Controller4;
        GameObject enemyPlayer5; //ne peut etre controlle par le joueur
        private PlayerController EnemyPlayer_Controller5;


        GameObject ball;
        Renderer ballRenderer; //renderer de la balle 

        Collider thisCollider; //collider de ce game object
        Renderer[] flechesRenderer = new Renderer[4]; //renderer des 4 fleches de ce game object
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
        Color cPasse;
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
            //mettre "0" en temps pour que le message reste a l'ecran

            if (Language.GetValue("language") == "EN")
            {
                tableau_1 = new string[,]
                {
                {"Welcome to the tutorial", "3" },
                {"There I will teach you \n how to play SuperSwungBall","4.5" },
                {"The aim is to score a touchdown \n behind the opponent's goal row","5" },
                {"To perform it, \n each player controls his team of Swungmen","4.5" },
                {"Let's start with the Swungmen's controls","3.5" }
                };

                tableau_2 = new string[,]
                {
                {"This is a Swungman, \n you can click on it in order to control it","4.5" },
                {"Every Swungmen have four abilities \n we will discover step by step","5" },
                {"First, the ability of displacement \n is represented by the blue color","5" },
                {"Press the blue button 3 times to make him run \n as fast and as far as possible","6" },
                {"You can click on the displacement zone (blue) \n to control its displacement","5.5" },
                {"You can now move there \n and press 'space' to lauch the displacement","0" }
                };

                tableau_3 = new string[,]
                {
                {"Well, now we'll see \n how to recover the ball", "4"} ,
                {"For this, \n you must at least have a ball-control point","5" },
                {"Put 1 point in the mastery of the ball (in pink) \n and 2 points in the displacement","6" },
                {"Now move to the ball \n ('Space' to start the displacement)","0" } }
                ;

                tableau_4 = new string[,]
                {
                {"We now have the ball, \n throw it somewhere","4" },
                {"To throw the ball, \n you must at least have a ball-control point","5" },
                {"Put 3 points of mastery in the pass (still pink) \n to make a pass as far as possible","6.5" },
                {"Click in the pink zone and then move the pink dot \n to mark the location of the pass","6.5" },
                {"You can start the action from now \n and press 'a' to make the pass","0" }
                };

                tableau_5 = new string[,]
                {
                {"What a pass! But take care, \n an enemy is coming","4" },
                {"He took the ball and wants to go this way. \n We must stop him.","5" },
                {"To engage in combat there is only one way: \n one of the two Swungmen must at least have tackle","7" },
                {"He certainly don't want to fight since he has the ball, \n so we have to engage in combat!","7" },
                {"Put two points in the tackle (green) \n and a point in the pass (to recover the ball if you tackle) \n and rush towards him","0" }
                };

                tableau_6 = new string[,]
                {
                {"There is a good tackle ! \n You went there so hard \n he ended down and has not finished its displacement","7.5" },
                {"Normally when an enemy has ended down \n it is weakened for the next round","5.5" },
                {"So I heal for the good of our training \n Easy life is for later","5.5" },
                {"It's now your turn to defend yourself \n because you took the ball","5.5" },
                {"It will surely try to tackle you \n to make you lose the ball","4.5" },
                {"So put maximum points in your dodge capability \n (in yellow) and rush towards him","0" }
                };

                tableau_7 = new string[,]
                {
                {"As expected he engaged in combat and we won \n because we had more \" fighting force \" than him","7" },
                {"The fighting force is the maximum value \n put in tackle OR dodging","5.5" },
                {"You must then be careful \n how to match these points","4" },
                {"Each character has different \n base stats in each mastery","5" },
                {"To be familiar with these stats is therefore crucial \n to not be surprised by the result of a fight","7.5" },
                {"Still, be careful not to do too much fighting \n because more fights you win, \n more you will be weakened for the next one","8.5" },
                {"To do a pass to a teammate, \n even if it is behind you, \n can be a good way to pass through the enemy defense ","7.5" },
                };

                tableau_8 = new string[,]
                {
                {"You now have all the keys \n to compare yourself to other players!","5" },
                {"To prove that you really have the level to, \n you must pass a final test","5.5" },
                {"You just have to score a touchdown","3" },
                {"Five new Swungmen appeared, \n one is in your team, \n feel free to use it","6" },
                {"You're free score the goal as you like \n it's time to put into practice what you have learned","0" }
                };

                tableau_9 = new string[,]
                {
                {"Great one !","2" },
                {"You've got a lot to learn to become the best \n anyways I see a great potential inside you, \n you will succeed","7" },
                {"There it's your time to show \n that you are worth confronting the players \n serious things are finally starting!","7" },
                {"Gl hf...","3" }
                };
            }//if langue
            else
            {
                tableau_1 = new string[,]
                {
                //{"Bienvenue dans le didacticiel", "3" },
                //{"C'est ici que je vais t'apprendre \n comment jouer a SuperSwungBall ","4.5" },
                //{"Le but du jeu est de marquer un touchdown \n derriere la ligne d'en-but adversaire ","5" },
                //{"Pour cela, chaque joueur controle \n les Swungmen de son equipe","4" },
                {"Commencons par voir les controles \n d'un Swungman","3.5" }
                };

                tableau_2 = new string[,]
                {
                //{"Ca, c'est un Swungman, \n tu peux cliquer dessus pour le controler","4" },
                //{"Tous les Swungmen ont quatre capacites \n que nous decouvrirons petit a petit","5" },
                //{"Pour commencer, la capacites de deplacement \n est representee par la couleur bleu","5" },
                //{"Appuie sur le bouton bleu 3 fois pour le faire \n courir le plus vite et le plus loin possible","5.5" },
                //{"Tu peux cliquer sur la zone de deplacement (en bleu) \n pour controler son deplacement","5.5" },
                {"Deplace le Swungman jusqu'ici \n et appuie sur 'Espace' pour lancer le deplacement","0" }
                };

                tableau_3 = new string[,]
                {
                //{"Bien, maintenant nous allons voir comment recuperer la balle", "4"} ,
                //{"Pour cela, \n il faut au moins un point de maitrise de la balle","5" },
                //{"Mets 1 point dans la maitrise de la balle (en rose) \n et 2 points dans la course","5" },
                {"Deplace toi maintenant vers la balle, \n ('Espace' pour lancer le deplacement)","0" } }
                ;

                tableau_4 = new string[,]
                {
                //{"Nous avons desormais la balle, \n envoyons-la quelque part","4" },
                //{"Pour envoyer la balle,\n il faut au moins un point de maitrise de la balle","5" },
                //{"Mets 3 points de maitrise dans la passe (toujours en rose) \n pour faire une passe le plus loin possible","6" },
                //{"Clique dans la zone rose, puis deplace le point rose \n pour marquer l'emplacement de la passe","5.5" },
                {"Tu peux desormais lancer l'action \n et appuyer sur 'a' pour faire la passe","0" }
                };

                tableau_5 = new string[,]
                {
                //{"Quelle passe ! Mais attention a toi, \n un ennemi arrive","4" },
                //{"Il a recupere la balle et veut continuer son chemin. \n Nous devons l'en empecher","5" },
                //{"Pour engager un combat il n'y a qu'un seul moyen : \n il faut qu'au moins un des deux joueurs possede du tacle","6" },
                //{"Il n'a surement pas envie de se battre puisqu'il a la balle, \n c'est donc a nous d'engager le combat !","5.5" },
                {"Mets deux points dans le tacle (en vert) \n et un point dans la passe (pour recuperer la balle si tu le tacles) \n et fonce vers lui","0" }
                };

                tableau_6 = new string[,]
                {
                //{"En voila un bon tacle ! \n Tu y es alle tellement fort \n qu'il a fini a terre et n'a pas termine son deplacement","6" },
                //{"En temps normal quand un ennemi fini a terre \n il est affaiblit pour le prochain tour","5.5" },
                //{"Je le soigne donc pour le bien de notre entrainement. \n La belle vie c'est pour plus tard","5.5" },
                //{"Mais desormais ca va etre a toi de te defendre \n car tu as recupere la balle","5" },
                //{"Il va surement essayer de te tacler \n pour te faire perdre la balle","4.5" },
                {"Mets donc un maximum de points dans ta capacite d'esquive \n (en jaune) et fonce vers lui","0" }
                };

                tableau_7 = new string[,]
                {
                //{"Comme prevu il a engage le combat et nous avons gagne \n car nous avions plus de \"force de combat\" que lui","6" },
                //{"La force de combat c'est la valeur maximale \n mise dans le tacle OU l'esquive ","5" },
                //{"Il faut donc faire attention a bien agencer ces points","4" },
                //{"Chaque personnage a des stats de base \n differentes dans chaque maitrise","5" },
                //{"Bien connaitre ces stats est donc primordiale \n pour ne pas etre surpris de l'issu d'un combat","5.5" },
                //{"Fait quand meme attention a ne pas trop abuser des combats \n car plus tu gagnes de combats, \n plus tu seras affaibli pour le prochain","7.5" },
                {"Faire la passe a un coequipier, \n meme s'il est derriere toi, \n peux ainsi etre un bon moyen de passer la defense ennemi","6.5" },
                };

                tableau_8 = new string[,]
                {
                //{"Tu as desormais toutes les armes en main \n pour te confronter aux autres joueurs !","5" },
                //{"Pour prouver que tu es vraiment a la hauteur, \n tu dois passer une ultime epreuve","5" },
                //{"Tu dois simplement marquer un touchedown","3" },
                //{"J'ai fait apparaitre cinq nouveaux Swungman, \n dont un est dans ton equipe, \n libre a toi de l'utiliser","6" },
                {"Tu es libre de marquer le but comme tu veux \n c'est l'heure de mettre en pratique tout ce que tu as appris","0" }
                };

                tableau_9 = new string[,]
                {
                //{"Super but !","2" },
                //{"Il te reste beaucoup a apprendre pour devenir le meilleur \n mais je vois en toi un grand potentiel, tu vas reussir","6.5" },
                //{"Il est temps de montrer ce que tu vaux en affrontant des joueurs ! \n les choses serieuses commencent enfin","6" },
                {"Bonne chance...","3" }
                };
            }// else langue
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

                case 2: //creation du player + player2(mais il n'est pas disponible pour le moment) & changement emplacement du texte & 
                    //le premier player
                    play_t0 = Settings.Instance.Default_player["itec"];
                    myPlayer = Instantiate(player1_prefab, new Vector3(1F, 1F, 0F), Quaternion.identity) as GameObject;
                    play_t0.Team_id = 0;
                    play_t0.Name = "#Test</§!";
                    myPlayer.name = play_t0.Name + "-" + play_t0.Team_id;
                    MyPlayer_Controller = (PlayerController)myPlayer.AddComponent(typeof(PlayerController));
                    MyPlayer_Controller.Player = play_t0;
                    MyPlayer_Controller.IsMine = true;
                    //le deuxieme player
                    play_t1 = Settings.Instance.Default_player["lombrix"];
                    myPlayer2 = Instantiate(player1_prefab, new Vector3(40F, 1F, 40F), Quaternion.identity) as GameObject; //trop loin pour etre vu
                    play_t1.Team_id = 0;
                    play_t1.Name = "Captain America";
                    myPlayer2.name = play_t1.Name + "-" + play_t1.Team_id;
                    MyPlayer2_Controller = (PlayerController)myPlayer2.AddComponent(typeof(PlayerController));
                    MyPlayer2_Controller.Player = play_t1;
                    MyPlayer2_Controller.IsMine = true;

                    screentext.transform.position = new Vector2(screentext.transform.position.x, 700);
                    screetext2.transform.position = new Vector2(screentext.transform.position.x, 697);
                    phase++;
                    break;

                case 3:
                    text(tableau_2);
                    break;

                case 4: //assignation des 4 bouttons + 3 couleurs & seulement boutton course dispo & fleches main + rond blanc visibles
                    BouttonEsquive = myPlayer.transform.FindChild("menu(Clone)").FindChild("boutton1").gameObject;
                    BouttonTacle = myPlayer.transform.FindChild("menu(Clone)").FindChild("boutton2").gameObject;
                    BouttonPasse = myPlayer.transform.FindChild("menu(Clone)").FindChild("boutton3").gameObject;
                    BouttonCourse = myPlayer.transform.FindChild("menu(Clone)").FindChild("boutton4").gameObject;

                    cEsquive = MyPlayer_Controller.menucontroller.GetButtonsColor[0]; //couleur de l'esquive
                    cTacle = MyPlayer_Controller.menucontroller.GetButtonsColor[1];   //couleur du tacle
                    cPasse = MyPlayer_Controller.menucontroller.GetButtonsColor[2];   //couleur de la passe
                    cCourse = MyPlayer_Controller.menucontroller.GetButtonsColor[3];  //couleur de la course

                    BouttonEsquive.SetActive(false);
                    BouttonPasse.SetActive(false);
                    BouttonTacle.SetActive(false);

                    foreach (Renderer r in flechesRenderer)
                        r.enabled = true;
                    RondBlanc.enabled = true;
                    phase++;
                    break;

                //5 = collision main & activation maitrise de la balle

                case 6:
                    text(tableau_3);
                    break;

                case 7: //balle visile & main sur la ball (c'est juste du visuel)
                    ballRenderer.enabled = true;
                    this.transform.position = ball.transform.position;
                    phase++;
                    break;

                case 8: //recuperation balle & desactivation boutton course
                    if (ball.transform.IsChildOf(myPlayer.transform))
                    {
                        end_time();
                        this.transform.position = new Vector3(0.56F, 0.31F, 0);
                        BouttonCourse.SetActive(false);
                        phase++;
                    }
                    break;

                case 9:
                    text(tableau_4);
                    break;

                //10 = passe : collision balle & activation esquive + desactivation des autres & creation 5 joueurs adverse (1 seul visilbe) & 3 courses pour le joueur

                case 11:
                    text(tableau_5);
                    break;

                //12 = deplacement joueur adverse & tacle par le player

                case 13: //3 courses pour le joueur & seulement esquive disponible
                    if (!annim_started)
                    {
                        BouttonEsquive.SetActive(true);
                        BouttonTacle.SetActive(false);
                        BouttonPasse.SetActive(false);

                        update_Stats(MyPlayer_Controller, cCourse, cCourse, cCourse);
                        phase++;
                    }
                    break;

                case 14:
                    text(tableau_6);
                    break;

                //15 = deplacement joueur adverse & esquive reussie par notre joueur

                case 16:
                    if (!annim_started)
                        text(tableau_7);
                    break;

                case 17: //tout les bouttons sont visibles pour le player & le player2 est la + il a la balle & changement de position player + adversaires
                    BouttonCourse.SetActive(true);
                    BouttonPasse.SetActive(true);
                    BouttonTacle.SetActive(true);

                    myPlayer.transform.position = new Vector3(-1.55F, 1, 15.69F);
                    myPlayer2.transform.position = new Vector3(5, 1, 16.21F);
                    enemyPlayer.transform.position = new Vector3(5, 1, 24);
                    enemyPlayer2.transform.position = new Vector3(-5, 1, 20);
                    enemyPlayer3.transform.position = new Vector3(9, 1, 20);
                    enemyPlayer4.transform.position = new Vector3(-3, 1, 12);
                    enemyPlayer5.transform.position = new Vector3(7, 1, 12);

                    MyPlayer_Controller.Player.BallHolder = false;
                    ball.transform.position = myPlayer2.transform.position;
                    ball.transform.parent = myPlayer2.transform.FindChild("perso");
                    MyPlayer2_Controller.Player.BallHolder = true;

                    phase++;
                    break;

                case 18:
                    text(tableau_8);
                    break;

                //19 = marquer un but

                case 20:
                    text(tableau_9);
                    break;

                case 21:
                    FadingManager.Instance.Fade();
                    break;
            } //switch
            time.update();
            if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
            {
                start_annim();
            }
        } //update


        // -- Text
        void text(string[,] tableau)
        {
            float temps = float.Parse(tableau[place, 1]);

            if (place == 0 && current_time == 0) // premier passage ? -> changer direct le texte
                message(tableau);

            if (temps == 0) // message qui reste a l'ecran ? laisser le texte a l'ecran et passer a la phase suivante
            {
                message(tableau);
                phase++;
                place = 0;
                current_time = 0;
            }
            else if (current_time < temps) // temps pas atteint ? -> continuer a afficher
                current_time += Time.deltaTime;
            else // temps atteint ? -> reinitialiser le temps et voir si on continue de lire le tableau
            {
                current_time = 0;
                if (tableau.GetLongLength(0) == place + 1) // dernier passage-> reinitialiser le texte et passer a la phase suivante
                {
                    screentext.text = "";
                    screetext2.text = "";
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
        void message(string[,] tableau)
        {
            screentext.text = tableau[place, 0];
            screetext2.text = tableau[place, 0];
        }
        // --

        // Collisions
        void OnTriggerEnter(Collider other)
        {
            GameObject objet = other.gameObject;
            if (objet == myPlayer && phase == 5) //collision avec le Player & premier deplacement ?
            {
                end_time();
                BouttonPasse.SetActive(true);
                this.transform.position = //new Vector3(6, 0, 0);
                    ball.transform.position;
                phase++;
            }
            if (objet == ball && phase == 10) //collision avec la balle & lancer la balle premiere fois ?
            {
                end_time();
                //main
                foreach (Renderer r in flechesRenderer)
                    r.enabled = false;
                RondBlanc.enabled = false;
                //myPlayer
                BouttonTacle.SetActive(true);
                BouttonCourse.SetActive(false);

                update_Stats(MyPlayer_Controller, cCourse, cCourse, cCourse);

                //enemyPlayer 1
                play_t2 = Settings.Instance.Default_player["pwc"];
                enemyPlayer = Instantiate(player2_prefab, new Vector3(ball.transform.position.x, 1F, ball.transform.position.z), Quaternion.identity) as GameObject;
                play_t2.Team_id = 1;
                play_t2.Name = "G1aD0s";
                EnemyPlayer_Controller = (PlayerController)enemyPlayer.AddComponent(typeof(PlayerController));
                EnemyPlayer_Controller.Player = play_t2;
                EnemyPlayer_Controller.IsMine = false;
                EnemyPlayer_Controller.settablePointDeplacement = true;
                //enemyPlayer 2
                play_t3 = Settings.Instance.Default_player["pwc"];
                enemyPlayer2 = Instantiate(player2_prefab, new Vector3(40, 1, 40), Quaternion.identity) as GameObject;
                play_t3.Team_id = 1;
                play_t3.Name = "Manus";
                EnemyPlayer_Controller2 = (PlayerController)enemyPlayer2.AddComponent(typeof(PlayerController));
                EnemyPlayer_Controller2.Player = play_t3;
                EnemyPlayer_Controller2.IsMine = false;
                EnemyPlayer_Controller2.settablePointDeplacement = true;
                //enemyPlayer 3
                play_t4 = Settings.Instance.Default_player["pwc"];
                enemyPlayer3 = Instantiate(player2_prefab, new Vector3(40, 1, 40), Quaternion.identity) as GameObject;
                play_t4.Team_id = 1;
                play_t4.Name = "Red";
                EnemyPlayer_Controller3 = (PlayerController)enemyPlayer3.AddComponent(typeof(PlayerController));
                EnemyPlayer_Controller3.Player = play_t4;
                EnemyPlayer_Controller3.IsMine = false;
                EnemyPlayer_Controller3.settablePointDeplacement = true;
                //enemyPlayer 4
                play_t5 = Settings.Instance.Default_player["pwc"];
                enemyPlayer4 = Instantiate(player2_prefab, new Vector3(40, 1F, 40), Quaternion.identity) as GameObject;
                play_t5.Team_id = 1;
                play_t5.Name = "ganondorf";
                EnemyPlayer_Controller4 = (PlayerController)enemyPlayer4.AddComponent(typeof(PlayerController));
                EnemyPlayer_Controller4.Player = play_t5;
                EnemyPlayer_Controller4.IsMine = false;
                EnemyPlayer_Controller4.settablePointDeplacement = true;
                //enemyPlayer 5
                Player play_t6 = Settings.Instance.Default_player["pwc"];
                enemyPlayer5 = Instantiate(player2_prefab, new Vector3(40, 1F, 40), Quaternion.identity) as GameObject;
                play_t6.Team_id = 1;
                play_t6.Name = "B. Nashor";
                EnemyPlayer_Controller5 = (PlayerController)enemyPlayer5.AddComponent(typeof(PlayerController));
                EnemyPlayer_Controller5.Player = play_t6;
                EnemyPlayer_Controller5.IsMine = false;
                EnemyPlayer_Controller5.settablePointDeplacement = true;
                phase++;
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
                if (MyPlayer2_Controller != null)
                    MyPlayer2_Controller.end_Anim();

                if (EnemyPlayer_Controller != null)
                    EnemyPlayer_Controller.end_Anim();
                if (EnemyPlayer_Controller2 != null)
                    EnemyPlayer_Controller2.end_Anim();
                if (EnemyPlayer_Controller3 != null)
                    EnemyPlayer_Controller3.end_Anim();
                if (EnemyPlayer_Controller4 != null)
                    EnemyPlayer_Controller4.end_Anim();
                if (EnemyPlayer_Controller5 != null)
                    EnemyPlayer_Controller5.end_Anim();

                annim_started = false;
                thisCollider.enabled = false; //desactiver le collider quand on est pas en phase d'animation
            }
        }
        private void start_annim()
        {
            thisCollider.enabled = true; //reactiver le collider quand on est en phase d'animation
            annim_started = true;
            time.start();
            MyPlayer_Controller.start_Anim();

            if (phase == 19) //le seul moment ou on a besoin du player2
                MyPlayer2_Controller.start_Anim();

            if (EnemyPlayer_Controller != null)
            {
                if (phase == 12)
                {//il se dirige vers le joueur #LaMeilleurIA
                    diriger_vers_player1(EnemyPlayer_Controller); //il se dirige vers le joueur #LaMeilleurIA

                    ball.transform.position = enemyPlayer.transform.position;
                    ball.transform.parent = enemyPlayer.transform.FindChild("perso");
                    MyPlayer2_Controller.Player.BallHolder = true;

                    update_Stats(EnemyPlayer_Controller, cCourse, cCourse, cCourse);
                }
                if (phase == 15)
                {
                    diriger_vers_player1(EnemyPlayer_Controller); //il se dirige vers le joueur #LaMeilleurIA
                    update_Stats(EnemyPlayer_Controller, cTacle, cCourse, cCourse);
                }
                if (phase == 19)
                {
                    diriger_vers_player2(EnemyPlayer_Controller); //il se dirige vers le joueur2 #LaMeilleurIA
                    update_Stats(EnemyPlayer_Controller, cTacle, cCourse, cEsquive);
                }

                //ennemi 2
                if (EnemyPlayer_Controller2 && phase == 19)
                {
                    diriger_vers_player1(EnemyPlayer_Controller2, 0, 0, 2); //il se dirige vers le joueur1 #LaMeilleurIA
                    update_Stats(EnemyPlayer_Controller2, cTacle, cCourse, cPasse);
                    EnemyPlayer_Controller2.start_Anim(false);
                }

                //ennemi 3
                if (EnemyPlayer_Controller3 && phase == 19)
                {
                    diriger_vers_player2(EnemyPlayer_Controller3, 0, 0, 2); //il se dirige vers le joueur2 #LaMeilleurIA
                    update_Stats(EnemyPlayer_Controller3, cTacle, cCourse, cCourse);
                    EnemyPlayer_Controller3.start_Anim(false);
                }

                //ennemi 4
                if (EnemyPlayer_Controller4 && phase == 19)
                {
                    diriger_vers_player1(EnemyPlayer_Controller4, 1, 0, 0); //il se dirige vers le joueur1 #LaMeilleurIA
                    update_Stats(EnemyPlayer_Controller4, cTacle, cCourse, cPasse);
                    EnemyPlayer_Controller4.start_Anim(false);
                }

                //ennemi 5
                if (EnemyPlayer_Controller5 && phase == 19)
                {
                    diriger_vers_player1(EnemyPlayer_Controller5, -1, 0, 0); //il se dirige vers le joueur1 #LaMeilleurIA
                    update_Stats(EnemyPlayer_Controller5, cTacle, cCourse, cCourse);
                    EnemyPlayer_Controller5.start_Anim(false);
                }
                EnemyPlayer_Controller.start_Anim(false); //animation du joueur ennemi
            }
        }
        public override void OnSucceedAttack(Player pl)
        {
            Debug.Log("attack player succeed : " + pl);
            if (pl == play_t0 && phase == 12)
                phase++;
        }
        public override void OnSucceedEsquive(Player p)
        {
            if (p == play_t0 && phase == 15)
                phase++;
        }


        public void update_score()
        {
            Debug.Log("update score + phase = " + phase);
            ball.transform.SetParent(myPlayer.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).transform);
            ball.transform.localPosition = new Vector3(-0.32f, 0.21f, -0.38f);
            ball.transform.localScale = new Vector3(1, 1, 1);
            if (phase == 19)
                phase++;
        }
        // --
        private void update_Stats(PlayerController player, Color c1, Color c2, Color c3)
        {
            player.updateValuesPlayer(c1);
            player.updateValuesPlayer(c2);
            player.updateValuesPlayer(c3);
        }
        private void diriger_vers_player1(PlayerController player, int modif_x = 0, int modif_y = 0, int modif_z = 0)
        {
            player.PointDeplacement = new Vector3(
                           myPlayer.transform.position.x + modif_x,
                           myPlayer.transform.position.y + modif_y,
                           myPlayer.transform.position.z + modif_z);
        }
        private void diriger_vers_player2(PlayerController player, int modif_x = 0, int modif_y = 0, int modif_z = 0)
        {
            player.PointDeplacement = new Vector3(
                          myPlayer2.transform.position.x + modif_x,
                          myPlayer2.transform.position.y + modif_y,
                          myPlayer2.transform.position.z + modif_z);
        }
    }//Class
}//Namespace
