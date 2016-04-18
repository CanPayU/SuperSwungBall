using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace Didacticiel
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private Text screentext;
        [SerializeField]
        private GameObject player1_prefab;
        [SerializeField]
        private GameObject player2_prefab;

        /*   
        private string screentext_text;
        private float startTime; 
        */
        Timer time;
        private float current_time;
        private string[,] tableau;
        private int place;

        // Use this for initialization
        void Start()
        {
            place = 0;
            current_time = 0;
            tableau = new string[,] {
            {"Bienvenue dans le didactitiel","2" },
            {"Comment jouer ?","3" },
            {"Le but du jeu est de marquer 3 points", "3" },
            {"Chaque joueur contrôle son équipe", "3" },
            {"Commençons par bouger un Swungman", "3" } };
            screentext.text = phrase();
            phase1();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void phase1()
        {
            if (current_time < temps())
                current_time += Time.deltaTime;
            else if (place < 5)
            {
                current_time = 0;
                place += 1;
                screentext.text = phrase();
            }
            else
            {
                phase2();
            }

        }
        void phase2()
        {
            Team team_0 = Game.Instance.Teams[0];
            Player play_t0 = Settings.Instance.Default_player["gpdn"];
            bool isMine = (PhotonNetwork.isMasterClient);
            GameObject play0 = Instantiate(player1_prefab, new Vector3(1F, 1F, 0F), Quaternion.identity) as GameObject;
            play_t0.Team_id = 0;
            play_t0.Name += "-" + 0;
            play0.name = play_t0.Name + "-" + play_t0.Team_id;
            PlayerController controller = play0.GetComponent<PlayerController>();
            controller.Player = play_t0;
            controller.IsMine = isMine;
        }
        string phrase()
        { return screentext.text = tableau[place, 0]; }
        float temps()
        { return float.Parse(tableau[place, 1]); }

    }
}