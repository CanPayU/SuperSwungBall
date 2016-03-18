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

        /*   
        private string screentext_text;
        private float startTime;
        */
        private float current_time;
        private string[,] tableau;
        private int place;
        int actions = 1;
        Player player;

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
            {"Commençons par bouger un Swungman", "3" }};
            phrase(tableau[place, 0]);
        }

        // Update is called once per frame
        void Update()
        {
            if (actions == 1)
            {
                if (current_time < time())
                    current_time += Time.deltaTime;
                else if (place == 4)
                {
                    ++actions;
                    phrase();
                }
                else
                {
                    current_time = 0;
                    ++place;
                    phrase(tableau[place, 0]);
                }
            }

            if (actions == 2)
            {
                player = new Player(10, 10, 10, 10, "Franck", 1);
                GameObject play0 = Instantiate(player1_prefab, new Vector3(0, 1F, 0), Quaternion.identity) as GameObject;
            }
        }

        void phrase(string s = "")
        { screentext.text = s; }
        float time()
        { return float.Parse(tableau[place, 1]); }

    }
}