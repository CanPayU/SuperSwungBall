using UnityEngine;
using System.Collections.Generic;

using GameKit;

namespace GameScene
{
	public class CollisionController : GameBehavior
    {
		ChatController chatController;
        Player player;
        PlayerController playerController; // évite le GetComponent<>()
        List<Collider> playerMet; // joueurs rencontré (uniquement ceux combattus) lors du tour. Un player de peut pas tacler 2 fois le même adversaire en 1 seul tour.
        bool goal;
        int premiereFrames;


        void Start()
        {
			if (PhotonNetwork.inRoom)
				chatController = GameObject.Find ("Main").GetComponent<ChatController> ();
            playerController = GetComponent<PlayerController>();
            player = playerController.Player;
            premiereFrames = 5;
            goal = false;
            playerMet = new List<Collider>();
        }

        public void OnTriggerStay(Collider other)
        {
			if (playerController != null && playerController.PhaseAnimation)
            {
                if (premiereFrames > 0)
                {
                    interception(other);
                    if (other.tag == "Player")
                    {
                        combat(other);
                    }
                    premiereFrames--;
                }
                if (!goal)
                {
                    Goal(other);
                }
            }
        }
        public void OnTriggerEnter(Collider other)
        {
			if (playerController != null && playerController.PhaseAnimation)
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
				Caller.Goal (g_controller);
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
                if (player.Tacle > player.Esquive)
                {
                    if (player.Tacle > attaqueAdverse)
                    {
						//Attaque Réussit
						if (PhotonNetwork.inRoom)
							chatController.InstanciateMessage (player.Name + " réussit son tacle !", ChatController.Chat.EVENT);
                        Debug.Log(name + "réussit son tacle!");
                        playerController.Animation("Attaque Reussit", 2f);
                        adversaireCollider.gameObject.GetComponent<CollisionController>().echec("Esquive");
						Caller.SuccessAttack (player);
					} else {
						Caller.FailedAttack (player);
					}
                }
                else
                {
					if (player.Esquive > attaqueAdverse) {
						//Esquive Réussit
						if (PhotonNetwork.inRoom)
							chatController.InstanciateMessage (player.Name + " réussit son esquive !", ChatController.Chat.EVENT);
						Debug.Log (name + "réussit son esquive!");
						playerController.Animation ("Esquive Reussit", 0.7f);
						adversaireCollider.gameObject.GetComponent<CollisionController> ().echec ("Attaque");
						Caller.SuccessEsquive (player);
					} else {
						Caller.FailedEsquive (player);
					}
                }
            }
        }

        public void echec(string animation)
        {
            bool porteurDeBall = transform.FindChild("perso").transform.FindChild("Ball") != null;
            playerController.Animation(animation + " Echec", 4);
            Debug.Log(name + " rate son" + animation);
            if (porteurDeBall)
			{
				if (PhotonNetwork.inRoom)
					chatController.InstanciateMessage (player.Name + " perd la balle !", ChatController.Chat.EVENT);
                Debug.Log(name + "perd la balle!");
                GameObject ball = transform.FindChild("perso").transform.FindChild("Ball").gameObject;
                ball.transform.localPosition = new Vector3(3.5f, 1.5f, -10f);
                ball.transform.parent = null;
                ball.GetComponent<Collider>().enabled = true;
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
