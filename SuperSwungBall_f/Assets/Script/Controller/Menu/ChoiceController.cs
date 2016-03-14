using UnityEngine;
using System.Collections;

namespace Menu
{
    public class ChoiceController : MonoBehaviour
    {



        private Transform trans;
        private GameObject ball;
        public RaycastHit hitP;

        private GameObject actual_player;








        public float journeyTime = 1.0F;
        private float startTime;

        private Vector3 center;
        private Vector3 riseRelCenter;
        private Vector3 setRelCenter;




        // Use this for initialization
        void Start()
        {



            startTime = Time.time;

            center = Vector3.zero;
            riseRelCenter = transform.position;
            setRelCenter = transform.position;

            ball = gameObject;
            trans = ball.GetComponent<Transform>();
            actual_player = null;
            hitP.point = transform.position;
        }

        // Update is called once per frame
        void Update()
        {

            Vector3 translation = new Vector3(0, 0);

            if (Input.GetButton("Horizontal"))
            {
                float speed = Input.GetAxisRaw("Horizontal");
                translation.x = (speed / 10);
            }
            if (Input.GetButton("Vertical"))
            {
                float speed = Input.GetAxisRaw("Vertical");
                translation.z = (speed / 10);
            }

            transform.Translate(translation);

            moovingToPoint();
        }






        private void moovingToPoint()
        {


            if (Input.GetKeyDown(KeyCode.Mouse0)) // clic droit
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // recuperation de la pos
                if (Physics.Raycast(ray, out hit, 100)) // sur un objet ?
                {
                    GameObject gm = hit.transform.gameObject;

                    if (gm.tag == "Play" && gm != actual_player) // sur un boutton Menu ? (ici Play)
                    {
                        Debug.Log("Clic on player");
                        trans.LookAt(hit.point);
                        hitP = hit;
                        actual_player = gm;

                        startTime = Time.time;
                        center = (hitP.point + trans.position) * 0.5F;
                        center -= new Vector3(0, 1, 0);
                        setRelCenter = hitP.point;
                        riseRelCenter = trans.position - center;
                        setRelCenter.y = hitP.point.y;
                    }
                }
            }
            float fracComplete = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);

        }


        void moveCurve()
        {



            float fracComplete = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        }
    }

}