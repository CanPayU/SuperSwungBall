using UnityEngine;
using System.Collections.Generic;

namespace GameScene
{
    public class CollisionController : MonoBehaviour
    {
        Player player;
        PlayerController playerController; // évite le GetComponent<>()
        List<Collider> playerMet; // joueurs rencontré (uniquement ceux combattus) lors du tour. Un player de peut pas tacler 2 fois le même adversaire en 1 seul tour.
        bool goal;
        int premiereFrames;
        void Start()
        {
            playerController = GetComponent<PlayerController>();
            player = playerController.Player;
            premiereFrames = 5;
            goal = false;
            playerMet = new List<Collider>();
        }

        public void OnTriggerStay(Collider other)
        {
            if (playerController.PhaseAnimation)
            {
                if (premiereFrames > 0)
                {
                    interception(other);
                    if (other.tag == "Player")
                    {
                        combat(other);
                    }
                    premiereFrames--;
                    Debug.Log(other);
                }
                if (!goal)
                {
                    Goal(other);
                }
            }
        }
        public void OnTriggerEnter(Collider other)
        {
            if (playerController.PhaseAnimation)
            {
                interception(other);
                if (other.tag == "Player")
                {
                    combat(other);
                }
            }
        }

        private void interception(Collider ballCollider)
        {
            // ramasse/intercepte la balle uniquement si le perso a au moins un élément "passe"
            if (ballCollider.transform.parent == null && ballCollider.name == "Ball" && player.ZonePasse != 0 && ballCollider.GetComponent<BallController>().interceptable(gameObject))
            {
                ballCollider.transform.parent = transform.FindChild("perso").transform;
                ballCollider.GetComponent<Collider>().enabled = false;
                ballCollider.transform.localPosition = new Vector3(1.3f, 3, 0);
            }
        }
        private void Goal(Collider goalCollider)
        {
            // GOAL
            if (goalCollider.tag == "Goal" && transform.FindChild("perso").transform.FindChild("Ball") != null)
            {
                transform.FindChild("perso").GetComponent<Animator>().Play("TouchDown");
                playerController.Pause = 10f;
                GoalController g_controller = goalCollider.GetComponent<GoalController>();
                g_controller.goal();
                goal = true;
            }
        }
        private void combat(Collider adversaireCollider)
        {
            Player adversaire = adversaireCollider.GetComponent<PlayerController>().Player;
            // collision adversaire et déclenchement combat
            if (adversaire.Team_id != player.Team_id && (adversaire.Tacle != 0 || player.Tacle != 0) && !playerMet.Contains(adversaireCollider))
            {
                // rotation des joueurs ( face à face)
                playerMet.Add(adversaireCollider);
                transform.FindChild("perso").transform.LookAt(new Vector3(adversaireCollider.transform.position.x, transform.FindChild("perso").position.y, adversaireCollider.transform.position.z));

                float attaqueAdverse = Mathf.Max(adversaire.Tacle, adversaire.Esquive);
                bool porteurDeBall = transform.FindChild("perso").transform.FindChild("Ball") != null;
                if (player.Tacle > player.Esquive)
                {
                    Debug.Log(player.Tacle);
                    if (player.Tacle > attaqueAdverse)
                    {
                        //Attaque réussit
                        playerController.Animation("Attaque Reussit", 2);
                        Debug.Log(name + " réussit son tacle");
                    }
                    else
                    {
                        //Attaque ratée
                        playerController.Animation("Attaque Echec", 4);
                        Debug.Log(name + " rate son tacle");

                    }
                }
                else
                {
                    Debug.Log(player.Esquive);
                    if (player.Esquive > attaqueAdverse)
                    {
                        //Esquive Réussit
                        playerController.Animation("Esquive Reussit", 1);
                        Debug.Log(name + " réussit son esquive");
                    }
                    else
                    {
                        //Esquive ratée
                        playerController.Animation("Esquive Echec", 4);
                        Debug.Log(name + " rate son esquive");
                    }
                }
            }
        }

        public void start_anim()
        {
            goal = false;
            premiereFrames = 5;
            playerMet.Clear();
        }
    }
}
