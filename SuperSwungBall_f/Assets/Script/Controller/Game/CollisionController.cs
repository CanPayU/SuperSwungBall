using UnityEngine;
using System.Collections;

namespace GameScene
{
    public class CollisionController : MonoBehaviour
    {
        Player player;
        bool goal;
        int premiereFrames;
        void Start()
        {
            player = GetComponent<PlayerController>().Player;
            premiereFrames = 5;
            goal = false;
        }

        public void OnTriggerStay(Collider other)
        {
            if (GetComponent<PlayerController>().PhaseAnimation)
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
            player = GetComponent<PlayerController>().Player;
            if (GetComponent<PlayerController>().PhaseAnimation)
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
                GetComponent<PlayerController>().Pause = 10f;
                GoalController g_controller = goalCollider.GetComponent<GoalController>();
                g_controller.goal();
                goal = true;
            }
        }
        private void combat(Collider adversaireCollider)
        {
            Player adversaire = adversaireCollider.GetComponent<PlayerController>().Player;
            // collision adversaire et déclenchement combat
            if (adversaire.Team_id != player.Team_id && (adversaire.Tacle != 0 || player.Tacle != 0))
            {
                // rotation des joueurs ( face à face)
                transform.FindChild("perso").transform.LookAt(new Vector3(adversaireCollider.transform.position.x, transform.FindChild("perso").position.y, adversaireCollider.transform.position.z));

                float attaqueAdverse = Mathf.Max(adversaire.Tacle, adversaire.Esquive);
                bool porteurDeBall = transform.FindChild("perso").transform.FindChild("Ball") != null;
                if (player.Tacle > player.Esquive)
                {
                    Debug.Log(player.Tacle);
                    if (player.Tacle > attaqueAdverse)
                    {
                        //Attaque réussit
                        transform.FindChild("perso").GetComponent<Animator>().Play("Attaque Reussit");
                        GetComponent<PlayerController>().Pause = 1f;
                        Debug.Log(name + " réussit son tacle");
                    }
                    else
                    {
                        //Attaque ratée
                        transform.FindChild("perso").GetComponent<Animator>().Play("Attaque Echec");
                        GetComponent<PlayerController>().Pause = 1f;
                        Debug.Log(name + " rate son tacle");

                    }
                }
                else
                {
                    Debug.Log(player.Esquive);
                    if (player.Esquive > attaqueAdverse)
                    {
                        //Esquive Réussit
                        transform.FindChild("perso").GetComponent<Animator>().Play("Esquive Reussit");
                        GetComponent<PlayerController>().Pause = 2f;
                        Debug.Log(name + " réussit son esquive");
                    }
                    else
                    {
                        //Esquive ratée
                        transform.FindChild("perso").GetComponent<Animator>().Play("Esquive Echec");
                        GetComponent<PlayerController>().Pause = 2f;
                        Debug.Log(name + " rate son esquive");
                    }
                }
            }
        }

        public void Start_anim()
        {
            goal = false;
            premiereFrames = 5;
        }
    }
}
