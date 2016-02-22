using UnityEngine;
using System.Collections;

namespace GameScene
{
    public class ButtonController : MonoBehaviour
    {
        //evite les "GetComponent<>"
        Color myColor;
        Collider myCollider;
		MenuController myMenu;
		PlayerController myPlayer;

        void Start()
        {
            myColor = GetComponent<Renderer>().material.color;
            myCollider = GetComponent<Collider>();
			myMenu = GetComponent<Transform>().parent.gameObject.GetComponent<MenuController>();
			myPlayer = GetComponent<Transform>().parent.parent.gameObject.GetComponent<PlayerController>();
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