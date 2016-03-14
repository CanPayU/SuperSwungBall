using UnityEngine;
using System.Collections;

namespace GameScene.Multi
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
			    passeur = transform.parent.gameObject;
        }

        void Update()
        {
			if (Input.GetKeyDown (KeyCode.Z)) {
				Debug.Log ("Info ball : passeur:" + passeur + " - parent:" + transform.parent);
			}
            if (!deplacement)
            {
                if (Input.GetKeyDown(KeyCode.A) && transform.parent != null)
                {
					deplacement = transform.parent.GetComponent<PlayerController>().passe(ref arrivalPoint); //Renvoit true si la passe est possible
                    if (deplacement)//debut de la passe
                    {
						PhotonView pv = PhotonView.Get (this);
						pv.RPC ("unattached_ball", PhotonTargets.All);
                    }
                }
            }
            else
            {
				passeur = null;
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

		[PunRPC] private void unattached_ball(){
			passeur = transform.parent.gameObject;
			transform.parent = null; // Detache la balle du joueur
		}

		void OnTriggerEnter(Collider other)
		{
			GameObject gmCol = other.gameObject;

			if (gmCol.CompareTag("Goal"))
			{
				GoalController g_controller = gmCol.GetComponent<GoalController> ();
				g_controller.goal ();
			}
		}
    }
}
