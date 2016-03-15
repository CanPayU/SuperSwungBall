﻿using UnityEngine;
using System.Collections;

namespace GameScene.Multi
{
	public class CollisionController : MonoBehaviour
	{
		Player player;
		void Start()
		{
			player = GetComponent<PlayerController>().Player;
			//Debug.Log (name + " PL:"+player.Name);
		}
		public void OnTriggerEnter(Collider other) //event collison
		{
			if (GetComponent<PlayerController>().Deplacement)
			{
				if (other.transform.parent == null && other.name == "Ball" && player.ZonePasse != 0 && other.GetComponent<BallController>().interceptable(gameObject)) // ramasse/intercepte la balle uniquement si le perso a au moins un élément "passe"
				{
					other.transform.parent = transform.FindChild("perso").transform;
					other.GetComponent<Collider>().enabled = false;
					other.transform.localPosition = new Vector3(1.3f, 3, 0);
				}

				if (other.tag == "Goal" && transform.FindChild("perso").transform.FindChild("Ball") != null) // GOAL
				{
					playAnnimation("TouchDown", 10f);
					GoalController g_controller = other.GetComponent<GoalController>();
					g_controller.goal();
				}

				if (other.tag == "Player")
				{
					Player adversaire = other.GetComponent<PlayerController>().Player;

					// Debug.Log (adversaire.Team_id); OK
					// Debug.Log (adversaire.Tacle); OK
					player = GetComponent<PlayerController>().Player; // NULL
					bool b = adversaire.Team_id != player.Team_id && (adversaire.Tacle != 0 || player.Tacle != 0);
					Debug.Log(adversaire.Team_id+"-"+player.Team_id+" - "+adversaire.Tacle+"-"+player.Tacle+" b:"+b);
					Debug.Log(adversaire.Team_id != player.Team_id && (adversaire.Tacle != 0 || player.Tacle != 0));

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
								playAnnimation("Attaque", 1f);
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
								GetComponent<PlayerController>().Pause = 2f;
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
			playAnnimation("Chute", 4f);

			if (porteurDeBall)
			{
				GameObject ball = transform.FindChild("perso").transform.FindChild("Ball").gameObject;
				Debug.Log(ball);
				ball.transform.localPosition -= new Vector3(0, 2.4f, 5);
				ball.GetComponent<Collider>().enabled = true;
				ball.transform.parent = null;
			}
		}

		private void playAnnimation(string name, float pause){
			PhotonView ph = gameObject.GetComponent <PhotonView> (); // View sur laquelle on fait l'annim
			ph.RequestOwnership ();
			PhotonView pv = PhotonView.Get (this);
			object[] param = new object[3];
			param [0] = ph.viewID; param [1] = name; param [2] = pause * 10;
			pv.RPC ("playAnnimationRPC", PhotonTargets.All, param); // sync
		}
		[PunRPC] private void playAnnimationRPC(object[] param){

			string name = param [0] as string;
			float pause = (float)((int)(param [2]) / 10f);
			int viewID = (int)param [1];

			GameObject other = PhotonView.Find (viewID).gameObject;
			other.transform.FindChild("perso").GetComponent<Animator>().Play(name);
			other.GetComponent<PlayerController>().Pause = pause;
		}
	}
}
