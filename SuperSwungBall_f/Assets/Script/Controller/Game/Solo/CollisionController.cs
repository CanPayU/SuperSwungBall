﻿using UnityEngine;
using System.Collections;

namespace GameScene.Solo
{
    public class CollisionController : MonoBehaviour
    {
        Player player;
        void Start()
        {
            player = GetComponent<Player_controller>().Player;
        }
        public void OnTriggerEnter(Collider other) //event collison
        {
            if (GetComponent<Player_controller>().PhaseAnimation)
            {
                if (other.transform.parent == null && other.name == "Ball" && player.ZonePasse != 0 && other.GetComponent<Ball_controller>().interceptable(gameObject)) // ramasse/intercepte la balle uniquement si le perso a au moins un élément "passe"
                {
                    other.transform.parent = transform.FindChild("perso").transform;
                    other.GetComponent<Collider>().enabled = false;
                    other.transform.localPosition = new Vector3(1.3f, 3, 0);
                }

                if (other.tag == "Goal" && transform.FindChild("perso").transform.FindChild("Ball") != null) // GOAL
                {
                    transform.FindChild("perso").GetComponent<Animator>().Play("TouchDown");
                    GetComponent<Player_controller>().Pause = 10f;
					GoalController g_controller = other.GetComponent<GoalController>();
                    g_controller.goal();
                }

                if (other.tag == "Player")
                {
                    Player adversaire = other.GetComponent<Player_controller>().Player;
                    if (adversaire.Team_id != player.Team_id && (adversaire.Tacle != 0 || player.Tacle != 0)) // collision adversaire et déclechement combat
                    {
                        transform.FindChild("perso").transform.LookAt(new Vector3(other.transform.position.x, transform.FindChild("perso").position.y, other.transform.position.z)); // rotation des joueurs ( face à face)

						int attaqueAdverse = (int)(Mathf.Max(adversaire.Tacle, adversaire.Esquive));
                        bool porteurDeBall = transform.FindChild("perso").transform.FindChild("Ball") != null;
                        if (player.Tacle > player.Esquive)
                        {
                            Debug.Log(player.Tacle);
                            if (player.Tacle > attaqueAdverse)
                            {
                                //animation Attaque
                                transform.FindChild("perso").GetComponent<Animator>().Play("Attaque");
                                GetComponent<Player_controller>().Pause = 1f;
                                Debug.Log(name + " réussit son tacle");
                            }
                            else
                            {
                                combatPerdu(porteurDeBall);
                                Debug.Log(name + " rate son tacle");
                                
                            }
                        }
                        else
                        {
                            Debug.Log(player.Esquive);
                            if (player.Esquive > attaqueAdverse)
                            {

                                //transform.FindChild("perso").GetComponent<Animator>().Play("Esquive");
                                GetComponent<Player_controller>().Pause = 2f;
                                Debug.Log(name + " réussit son esquive");
                            }
                            else
                            {
                                combatPerdu(porteurDeBall);
                                Debug.Log(name + " rate son esquive");
                            }
                        }
                    }
                }
            }
        }

        private void combatPerdu(bool porteurDeBall)
        {
            //animation Chute
            transform.FindChild("perso").GetComponent<Animator>().Play("Chute");
            GetComponent<Player_controller>().Pause = 4f;

            if (porteurDeBall)
            {
                GameObject ball = transform.FindChild("perso").transform.FindChild("Ball").gameObject;
                Debug.Log(ball);
                ball.transform.localPosition -= new Vector3(0, 2.4f, 5);
                ball.GetComponent<Collider>().enabled = true;
                ball.transform.parent = null;
            }
        }
    }
}
