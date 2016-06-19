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

        private KeyCode keyPasse; // Touche utilisé pour la passe

        void Start()
        {
            this.keyPasse = Settings.Instance.Keyboard[KeyboardAction.Passe];
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
                if (Input.GetKeyDown(keyPasse) && transform.parent != null)
                {
                    TriggerPasse();
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
                    Debug.Log("Deplacement");
                    transform.position = Vector3.MoveTowards(transform.position, arrivalPoint, Time.deltaTime * 15f);// mouvement de la passe
                }
            }


        }
        public bool interceptable(GameObject player) // evite le passeur d'intercepter sa propre balle, renvoit false si l'intercepteur est le lanceur
        {
            return player != passeur || !deplacement;
        }

        private void TriggerPasse()
        {
            if (PhotonNetwork.inRoom)
            {
                PhotonView.Get(this).RPC("ExecutePasse", PhotonTargets.All);
            }
            else
            {
                ExecutePasse();
            }
        }
        [PunRPC]
        private void ExecutePasse()
        {
            deplacement = transform.parent.parent.GetComponent<BasicPlayerController>().passe(ref arrivalPoint); //Renvoit true si la passe est possible
            if (deplacement)//debut de la passe
            {
                GetComponent<Collider>().enabled = true;
                passeur = transform.parent.parent.gameObject;
                transform.parent = null; // Detache la balle du joueur

                Debug.Log("In BC:" + arrivalPoint);

                // animation passe
                passeur.transform.FindChild("perso").LookAt(new Vector3(arrivalPoint.x, passeur.transform.FindChild("perso").position.y, arrivalPoint.z));
                passeur.GetComponent<BasicPlayerController>().Animation("Passe", 0.7f);
            }
        }
        public void LacheBalle()
        {
            deplacement = true;
            GetComponent<Collider>().enabled = true;
            passeur = transform.parent.parent.gameObject;
            transform.localPosition = new Vector3(3.5f, 1.5f, -10f);
            arrivalPoint = transform.position;
            transform.localPosition = new Vector3(1.3f, 3, 0);
            transform.parent = null; // Detache la balle du joueur
            Debug.Log("perd la balle");
        }
    }
}
