using UnityEngine;
using System.Collections;
using System.Threading;

namespace GameScene
{
	public class BallController : MonoBehaviour
	{
		private GameObject passeur; //joueur possédant la balle juste avant la passe
		private Vector3 arrivalPoint; // point d'arrivée de la passe
		private bool deplacement; // Balle en l'air

		void Start()
		{
			deplacement = false;
			if (transform.parent != null)
			{
				passeur = transform.parent.gameObject;
				GetComponent<Collider>().enabled = false;
			}
		}

		void Update()
		{
			if (!deplacement)
			{
				if (Input.GetKeyDown(KeyCode.A) && transform.parent != null)
				{
					TriggerPasse ();
				}
			}
			else
			{
				if (transform.position == arrivalPoint || transform.parent != null)// fin de la passe ou interception
				{
					deplacement = false;
				}
				if (deplacement)
				{
					transform.position = Vector3.MoveTowards(transform.position, arrivalPoint, Time.deltaTime * 20);// mouvement de la passe
				}
			}


		}
		public bool interceptable(GameObject player) // evite le passeur d'intercepter sa propre balle, renvoit false si l'intercepteur est le lanceur
		{
			return player != passeur || !deplacement;
		}

		private void TriggerPasse(){
			if (PhotonNetwork.inRoom) {
				PhotonView.Get (this).RPC ("ExecutePasse", PhotonTargets.All);
			} else {
				ExecutePasse ();
			}
		}
		[PunRPC] private void ExecutePasse(){
			deplacement = transform.parent.parent.GetComponent<PlayerController>().passe(ref arrivalPoint); //Renvoit true si la passe est possible
			if (deplacement)//debut de la passe
			{
				GetComponent<Collider>().enabled = true; 
				passeur = transform.parent.parent.gameObject;
				transform.parent = null; // Detache la balle du joueur

				Debug.Log ("In BC:" + arrivalPoint);

				// animation passe
				passeur.transform.FindChild("perso").LookAt(new Vector3(arrivalPoint.x, passeur.transform.FindChild("perso").position.y, arrivalPoint.z));
				passeur.transform.FindChild("perso").GetComponent<Animator>().Play("Passe");
				passeur.GetComponent<PlayerController>().Pause = 0.7f;
			}
		}
	}
}
