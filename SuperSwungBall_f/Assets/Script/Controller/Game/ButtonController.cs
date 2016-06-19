using UnityEngine;
using System.Collections;
using GameScene.Solo;
using GameScene.Multi;

namespace GameScene
{
    public class ButtonController : MonoBehaviour
    {
        //evite les "GetComponent<>"
        Color myColor;
        Collider myCollider;
        MenuController myMenu;

        //Clic event
        Ray ray;
        RaycastHit hit;

        void Start()
        {
            myColor = GetComponent<Renderer>().material.color;
            myCollider = GetComponent<Collider>();
            myMenu = GetComponent<Transform>().parent.gameObject.GetComponent<MenuController>();
            transform.TransformPoint(1, 0, 0);
        }

        void OnMouseEnter() // event souris entre
        {
            //transform.
            transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
        }

        void OnMouseOver() // event souris dessus
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit, 100);
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!hit.Equals(null))
                {
                    if (hit.collider == myCollider) // Collision clic
                    {
                        myMenu.update_Color(myColor); // Change la couleur des valeurs

						GetComponent<Transform>().parent.parent.gameObject.GetComponent<BasicPlayerController>().updateValuesPlayer(myColor);
                        /*
						if (PhotonNetwork.inRoom) {
							GetComponent<Transform> ().parent.parent.gameObject.GetComponent<PlayerController> ().updateValuesPlayer (myColor);
						} else {
							GetComponent<Transform>().parent.parent.gameObject.GetComponent<Player_controller>().updateValuesPlayer(myColor);
						}*/
                        transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
            }
        }

        void OnMouseExit() // event souris quitte
        {
            transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
        }
    }
}