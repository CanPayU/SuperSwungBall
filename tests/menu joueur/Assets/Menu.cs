using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour
{

    Color myColor;
    Collider myCollider;
    Player myPlayer;
    void Start()
    {
        myColor = GetComponent<Renderer>().material.color;
        myCollider = GetComponent<Collider>();
        myPlayer = GetComponent<Transform>().parent.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider == myCollider)
                {
                    myPlayer.changer_valeur(myColor);
                }
            }
        }
    }
}
