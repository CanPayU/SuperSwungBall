using UnityEngine;
using System.Collections;

namespace Assets
{
    public class Button_controller : MonoBehaviour
    {
        //evite les "GetComponent<>"
        Color myColor;
        Collider myCollider;
        Menu_controller myMenu;
        Player_controller myPlayer;

        void Start()
        {
            myColor = GetComponent<Renderer>().material.color;
            myCollider = GetComponent<Collider>();
            myMenu = GetComponent<Transform>().parent.gameObject.GetComponent<Menu_controller>();
            myPlayer = GetComponent<Transform>().parent.parent.gameObject.GetComponent<Player_controller>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
 
                if (Physics.Raycast(ray, out hit, 100)) 
                {
                    if (hit.collider == myCollider) // Collision clic
                    {
                        myMenu.update_Color(myColor); // Change la couleur des valeurs
                        myPlayer.updateValuesPlayer(myColor); // Change les stats du perso
                    }
                }
            }
        }
    }
}