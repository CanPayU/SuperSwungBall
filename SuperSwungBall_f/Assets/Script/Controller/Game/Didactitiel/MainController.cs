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
        [SerializeField]
        private GameObject player1_prefab;
        [SerializeField]
        private GameObject player2_prefab;


        CameraController cameraController;

        InfoJoueurController infoJoueur;
        // Panel info joueur​

        Timer time;
        private float current_time;
        private string[,] tableau_1;
        private string[,] tableau_2;
        private int place;
        private int phase;
        private bool premier_passage = true;

        private bool annim_started = false;
        private PlayerController player_phase_2;

        // Use this for initialization
        void Start()
        {
            cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
            phase = 1;
            place = 0;
            current_time = 0;
            time = new Timer(10.0F, end_time);
            //text
            tableau_1 = new string[,] {
            {"Bienvenue dans le didactitiel","1" },
            {"Comment jouer ?","1" }, 
          //  {"Le but du jeu est de marquer 3 points", "1" }, 
            //{"Chaque joueur contrôle son équipe", "1" }, 
            //{"Commençons par voir les contrôles \n d'un Swungman", "1" }, 
            {"", "0" }};

            tableau_2 = new string[,] {
            {"Ca c'est un Swungman, \n appuie dessus pour pouvoir le contrôler","1" },
           {"Les capacités de déplacements sont représentées \n par la couleur bleu", "1" },
         //   {"Appuie sur le bouton bleu 3 fois pour le faire \n courire le plus vite et le plus loin possible", "1" },
            {"Déplace le Swungman jusqu'ici", "1"},
            {"","0" } };

            screentext.text = message(tableau_1);
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
            }
            time.update();
            if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
            {
                start_annim();
            }
        }
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
            this.player_phase_2 = play0.GetComponent<PlayerController>();
            this.player_phase_2.Player = play_t0;
            this.player_phase_2.IsMine = true;
            phase++;
        }
        void phase3()
        {
            if (premier_passage)
            {
                screentext.transform.position = new Vector2(930, 700);
                premier_passage = false;
                screentext.text = message(tableau_2);
            }
            text(tableau_2);
        }
        void phase4()
        {
            this.GetComponent<Renderer>().material.SetColor("_SpecColor", Color.red);
        }

        void text(string[,] tableau)
        {
            float temp = temps(tableau);

            if (temp == 0) //dernier passage
            {
                phase++;
                current_time = 0;
                place = 0;
            }
            else if (current_time < temp) //continuer à afficher
                current_time += Time.deltaTime;
            else // changer de texte
            {
                current_time = 0;
                place += 1;
                screentext.text = message(tableau);
            }
        }
        string message(string[,] tableau)
        {
            return screentext.text = tableau[place, 0];
        }

        float temps(string[,] tableau)
        {
            return float.Parse(tableau[place, 1]);
        }

        private void end_time()
        {
            time.reset();
            if (annim_started)
            {
                this.player_phase_2.end_Anim();
                annim_started = false;
            }
        }
        private void start_annim()
        {
            annim_started = true;
            time.start();
            this.player_phase_2.start_Anim();
        }
    }
}
