using UnityEngine;
using System.Collections;

namespace GameScene
{
    public class Ball_controller : MonoBehaviour
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
            if (!deplacement)
            {
                if (Input.GetKeyDown(KeyCode.A) && transform.parent != null)
                {
                    deplacement = transform.parent.GetComponent<Player_controller>().passe(ref arrivalPoint); //Renvoit true si la passe est possible
                    if (deplacement)//debut de la passe
                    {
                        passeur = transform.parent.gameObject;
                        transform.parent = null; // Detache la balle du joueur
                    }
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

		void OnTriggerEnter(Collider other)
		{
			GameObject gmCol = other.gameObject; // gm touche
                                                 /*if (gmCol.CompareTag("Player"))
                                                 {
                                                     passed = false;
                                                     holder = gmCol; // devient le parent
                                                     gameObject.transform.SetParent(holder.transform);
                                                     transform.localPosition = new Vector3(0, 0.8f,0); // se met sur lui
                                                 }else */

			if (gmCol.CompareTag("Goal"))
			{
                
				GoalController g_controller = gmCol.GetComponent<GoalController> ();
				g_controller.goal ();
			}
		}
    }
}
