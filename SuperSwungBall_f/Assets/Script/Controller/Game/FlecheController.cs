using UnityEngine;
using System.Collections;

namespace GameScene
{
    public class FlecheController : MonoBehaviour
    {

        Renderer bout;
        Renderer centre;
        Renderer pointe;

        Vector3 POS_INITIALE = new Vector3(0, -0.4f, 0);

        void Start()
        {
            bout = transform.GetChild(0).GetComponent<Renderer>();
            centre = transform.GetChild(1).GetComponent<Renderer>();
            pointe = transform.GetChild(2).GetComponent<Renderer>();
            reset();
        }

        public void reset()
        {
            bout.material.color = new Color(1, 0.5f, 0);
            centre.material.color = new Color(1, 0.5f, 0);
            pointe.material.color = new Color(1, 0.5f, 0);
            transform.localScale = new Vector3(0, transform.localScale.y, transform.localScale.z);
            transform.localPosition = POS_INITIALE;
        }

        public void display(bool afficher)
        {
            bout.enabled = afficher;
            centre.enabled = afficher;
            pointe.enabled = afficher;
        }

        public void changeColor(Color c)
        {
            pointe.material.color = centre.material.color;
            centre.material.color = bout.material.color;
            bout.material.color = c;
        }

        public void point(Vector2 pos)
        {
            transform.localPosition = POS_INITIALE;
            transform.localScale = new Vector3(Vector2.Distance(new Vector2(transform.parent.position.x, transform.parent.position.z), pos) / 7f, transform.localScale.y, transform.localScale.z);
            Debug.Log(transform.localScale.x);
            transform.LookAt(new Vector3(pos.x, transform.position.y, pos.y));
            transform.localEulerAngles += new Vector3(0, 90, 0);
            transform.position = new Vector3(pos.x, transform.position.y, pos.y);
        }
    }
}
