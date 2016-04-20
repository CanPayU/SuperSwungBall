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
        private string[,] tableau;
        private int place;
        private int phase;

        /******************** EDITED **********************/
        private bool annim_started = false;
        private PlayerController player_phase_2;
        /******************************************/
        // Use this for initialization
        void Start()
        {
            cameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
            phase = 1;
            place = 0;
            current_time = 0;
            tableau = new string[,] {
            {"Bienvenue dans le didactitiel","1" }, //2
            {"Comment jouer ?","1" }, //3
            {"Le but du jeu est de marquer 3 points", "1" }, //3
            {"Chaque joueur contrôle son équipe", "1" }, //3
            {"Commençons par bouger un Swungman", "1" }, //3
            {"", "0" }};
            screentext.text = phrase();
            phase1();
            /******************* EDITED ***********************/
            time = new Timer(10.0F, end_time);
            /******************************************/
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
                    break;
            }
            /********************** EDITED ********************/
            time.update();
            if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
            {
                start_annim();
            }
            /******************************************/
        }
        void phase1()
        {
            if (temps() == 0)
            {
                phase++;
                phase2();
                /***************** EDITED *************************/
                return;
                /******************************************/
            }
            if (current_time < temps())
                current_time += Time.deltaTime;
            else
            {
                current_time = 0;
                place += 1;
                screentext.text = phrase();
            }
        }
        void phase2()
        {
            /******************* EDITED ***********************/
            //Team team_0 = Game.Instance.Teams[0];
            Player play_t0 = Settings.Instance.Default_player["gpdn"];
            //bool isMine = (PhotonNetwork.isMasterClient);
            GameObject play0 = Instantiate(player1_prefab, new Vector3(1F, 1F, 0F), Quaternion.identity) as GameObject;
            play_t0.Team_id = 0;
            play_t0.Name += "-" + 0;
            play0.name = play_t0.Name + "-" + play_t0.Team_id;
            this.player_phase_2 = play0.GetComponent<PlayerController>();
            this.player_phase_2.Player = play_t0;
            this.player_phase_2.IsMine = true; //ismine
                                               /******************************************/
        }
        string phrase()
        { return screentext.text = tableau[place, 0]; }
        float temps()
        { return float.Parse(tableau[place, 1]); }
        /********************* EDITED ************************************************/
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
        /******************************************/
    }
}
