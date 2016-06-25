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

        Player play_t0; //le player
        GameObject myPlayer;
        GameObject BouttonEsquive;
        GameObject BouttonTacle;
        GameObject BouttonPasse;
        GameObject BouttonCourse;
        private PlayerController MyPlayer_Controller;

        Player play_t1; //le deuxième player
        GameObject myPlayer2;
        private PlayerController MyPlayer2_Controller;

        Player play_t2;
        GameObject enemyPlayer; //ne peut être controllé par le joueur
        private PlayerController EnemyPlayer_Controller;

        Player play_t3;
        GameObject enemyPlayer2; //ne peut être controllé par le joueur
        Player play_t4;
        private PlayerController EnemyPlayer_Controller2;
        GameObject enemyPlayer3; //ne peut être controllé par le joueur
        Player play_t5;
        private PlayerController EnemyPlayer_Controller3;
        GameObject enemyPlayer4; //ne peut être controllé par le joueur
        Player play_t6;
        private PlayerController EnemyPlayer_Controller4;
        GameObject enemyPlayer5; //ne peut être controllé par le joueur
        private PlayerController EnemyPlayer_Controller5;


        GameObject ball;
        Renderer ballRenderer; //renderer de la balle 

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
            //mettre "0" en temps pour que le message reste à l'écran

            tableau_1 = new string[,]
            {
                //{"Bienvenue dans le didacticiel", "3" },
                //{"C'est ici que je vais t'apprendre \n comment jouer à SuperSwungBall","4.5" },
                //{"Le but du jeu est de marquer un touchdown \n derrière la ligne d'en-but adversaire ","5" },
                //{"Pour cela, chaque joueur contrôle \n les Swungmen de son équipe","4" },
                {"Commençons par voir les contrôles \n d'un Swungman","3.5" }
            };

            tableau_2 = new string[,]
            {
                //{"Ça, c'est un Swungman, \n tu peux cliquer dessus pour le contrôler","4" },
                //{"Tout les Swungmen ont quatre capacités \n que nous découvrirons petit à petit","5" },
                //{"Pour commencer, la capacités de déplacement \n est représentée par la couleur bleu","5" },
                //{"Appuie sur le bouton bleu 3 fois pour le faire \n courir le plus vite et le plus loin possible","5.5" },
                //{"Tu peux cliquer sur la zone de déplacement (en bleu) \n pour contrôler son déplacement","5.5" },
                {"Déplace le Swungman jusqu'ici \n et appuie sur 'Espace' pour lancer le déplacement","0" }
            };

            tableau_3 = new string[,]
            {
                //{"Bien, maintenant nous allons voir comment récupérer la balle", "4"} ,
                //{"Pour cela, \n il faut au moins un point de maîtrise de la balle","5" },
                //{"Mets 1 point dans la maîtrise de la balle (en rose) \n et 2 points dans la course","5" },
                {"Déplace toi maintenant vers la balle, \n ('Espace' pour lancer le déplacement)","0" } }
            ;

            tableau_4 = new string[,]
            {
                //{"Nous avons désormais la balle, envoyons-la quelque part","4" },
                //{"Pour envoyer la balle,\n il faut au moins un point de maîtrise de la balle","5" },
                //{"Mets 3 points de maîtrise dans la passe (toujours en rose) \n pour faire une passe le plus loin possible","6" },
                //{"Clique dans la zone rose, puis déplace le point rose \n pour marquer l'emplacement de la passe","5.5" },
                {"Tu peux désormais lancer l'action \n et appuyer sur 'a' pour faire la passe","0" }
            };

            tableau_5 = new string[,]
            {
                //{"Quelle passe ! Mais attention à toi, \n un ennemi arrive","4" },
                //{"Il a récupéré la balle et veut continuer son chemin. \n Nous devons l'en empêcher","5" },
                //{"Pour engager un combat il n'y a qu'une seul moyen : \n il faut qu'au moins un des deux joueurs possède du tacle","6" },
                //{"Il n'a sûrement pas envie de se battre puisqu'il a la balle, \n c'est donc à nous d'engager le combat !","5.5" },
                {"Mets deux points dans le tacle (en vert) \n et un point dans la passe (pour récupérer la balle si tu le tacles) \n et fonce vers lui","0" }
            };

            tableau_6 = new string[,]
            {
            //    {"En voilà un bon tacle ! \n Tu y es allé tellement fort \n qu'il a fini à terre et n'a pas terminé son déplacement","6" },
            //    {"En temps normal quand un ennemi finit à terre \n il est affaiblit pour le prochain tour","5.5" },
            //    {"Je le soigne donc pour le bien de notre entraînement. \n La belle vie c'est pour plus tard","5.5" },
            //    {"Mais désormais ça va être à toi de te défendre \n car tu as récupéré la balle","5" },
            //    {"Il va sûrement essayer de te tacler \n pour te faire perdre la balle","4.5" },
                {"Mets donc un maximum de points dans ta capacité d'esquive \n (en jaune) et fonce vers lui","0" }
            };

            tableau_7 = new string[,]
            {
                //{"Comme prévu il a engagé le combat et nous avons gagné \n car nous avions plus de \"force de combat\" que lui","6" },
                //{"La force de combat c'est la valeur maximale \n mise dans le tacle OU l'esquive ","5" },
                //{"Il faut donc faire attention à bien agencer ces points","4" },
                //{"Chaque personnage a des stats de base \n différentes dans chaque maîtrise","5" },
                //{"Bien connaître ces stats est donc primordiale \n pour ne pas être surpris de l'issu d'un combat","5.5" },
                //{"Fait quand même attention à ne pas trop abuser des combats \n car plus tu gagnes de combats, \n plus tu seras affaibli pour le prochain","7.5" },
                {"Faire la passe à un coéquipier, \n même s'il est derrière nous, \n peux ainsi être un bon moyen de passer la défense ennemi","6.5" },
            };

            tableau_8 = new string[,]
            {
                //{"Tu as désormais toutes les armes en main \n pour te confronter aux autres joueurs !","5" },
                //{"Pour prouver que tu es vraiment à la hauteur, \n tu dois passer une ultime épreuve","5" },
                //{"Tu dois simplement marquer un but","3" },
                //{"J'ai aussi fait apparaître un nouveau Swungman, \n il est dans ton équipe, \n libre à toi de l'utiliser","6" },
                {"Tu es libre de marquer le but comme tu veux \n c'est l'heure de mettre en pratique tout ce que tu as appris","0" }
            };

            tableau_9 = new string[,]
            {
                //{"Super but !","2" },
                //{"Il te reste beaucoup à apprendre pour devenir le meilleur \n mais je vois en toi un grand potentiel, tu vas réussir","6.5" },
                //{"Il temps de montrer ce que tu vaux en affrontant des joueurs ! \n les choses sérieuses commencent enfin","6" },
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

                case 2: //création du player + player2(mais il n'est pas disponible pour le moment) & changement emplacement du texte & 
                    //le premier player
                    play_t0 = Settings.Instance.Default_player["itec"];
                    myPlayer = Instantiate(player1_prefab, new Vector3(1F, 1F, 0F), Quaternion.identity) as GameObject;
                    play_t0.Team_id = 0;
                    play_t0.Name = "#Test</§!";
                    myPlayer.name = play_t0.Name + "-" + play_t0.Team_id;
                    MyPlayer_Controller = (PlayerController)myPlayer.AddComponent(typeof(PlayerController));
                    MyPlayer_Controller.Player = play_t0;
                    MyPlayer_Controller.IsMine = true;
                    //le deuxième player
                    play_t1 = Settings.Instance.Default_player["lombrix"];
                    myPlayer2 = Instantiate(player1_prefab, new Vector3(40F, 1F, 40F), Quaternion.identity) as GameObject; //trop loin pour être vu
                    play_t1.Team_id = 0;
                    play_t1.Name = "Captain America";
                    myPlayer2.name = play_t1.Name + "-" + play_t1.Team_id;
                    MyPlayer2_Controller = (PlayerController)myPlayer2.AddComponent(typeof(PlayerController));
                    MyPlayer2_Controller.Player = play_t1;
                    MyPlayer2_Controller.IsMine = true;

                    screentext.transform.position = new Vector2(960, 700);
                    phase++;
                    break;

                case 3:
                    text(tableau_2);
                    break;

                case 4: //assignation des 4 bouttons + 3 couleurs & seulement boutton course dispo & flèches main + rond blanc visibles
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

                case 8: //récupération balle & désactivation boutton course
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

                //10 = passe : collision balle & activation esquive + désactivation des autres & création 5 joueurs adverse (1 seul visilbe) & 3 courses pour le joueur

                case 11:
                    text(tableau_5);
                    break;

                //12 = déplacement joueur adverse & tacle par le player

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

                //15 = déplacement joueur adverse & esquive réussie par notre joueur

                case 16:
                    if (!annim_started)
                        text(tableau_7);
                    break;

                case 17: //tout les bouttons sont visibles pour le player & le player2 est là + il a la balle & changement de position player + adversaires
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
            if (objet == myPlayer && phase == 5) //collision avec le Player & premier déplacement ?
            {
                end_time();
                BouttonPasse.SetActive(true);
                this.transform.position = //new Vector3(6, 0, 0);
                    ball.transform.position;
                phase++;
            }
            if (objet == ball && phase == 10) //collision avec la balle & lancer la balle première fois ?
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
                thisCollider.enabled = false; //désactiver le collider quand on est pas en phase d'animation
            }
        }
        private void start_annim()
        {
            thisCollider.enabled = true; //réactiver le collider quand on est en phase d'animation
            annim_started = true;
            time.start();
            MyPlayer_Controller.start_Anim();

            if (phase == 19) //le seul moment où on a besoin du player2
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
