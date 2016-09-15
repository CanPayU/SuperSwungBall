using UnityEngine;
using System.Collections;

namespace Menu
{
    public class ChoiceController : MonoBehaviour
    {
        private GameObject ball;
        private Transform trans;

        private GameObject current_player;

        private float journeyTime = 1.0F;
        private float startTime;

        private Vector3 center;
        private Vector3 riseRelCenter;
        private Vector3 setRelCenter;

        private GameObject main_Camera;
        private GameObject support_Ball;

        // Use this for initialization
        void Start()
        {
            startTime = Time.time;

            center = Vector3.zero;
            riseRelCenter = transform.position;
            setRelCenter = transform.position;

            ball = gameObject;
            trans = ball.GetComponent<Transform>();
            current_player = null;

            main_Camera = GameObject.Find("Main Camera");
            support_Ball = GameObject.Find("Support_Ball");
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
            if (Input.GetKeyDown(KeyCode.Mouse0) && ApplicationModel.BackgroundSceneActionAllowed) // clic droit
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // recuperation de la pos
                if (Physics.Raycast(ray, out hit, 100)) // sur un objet ?
                {
                    GameObject gm = hit.transform.gameObject;

                    if (gm.tag == "Play" && gm != current_player) // sur un boutton Menu ? (ici Play)
                    {
                        // -- Désactivation des boutons de choix de team
                        setActive_ButtonTeam(false);
                        // --
                        trans.LookAt(hit.point);
                        current_player = gm;

                        startTime = Time.time;
                        center = (hit.point + trans.position) * 0.5F - new Vector3(0, 1, 0) + new Vector3(0, 0.5F, 0);

                        setRelCenter = hit.point - center;
                        riseRelCenter = trans.position - center;
                    }
                }
            }
            float fracComplete = (Time.time - startTime) / journeyTime;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete) + center;
            if (current_player == support_Ball)
            {
                setActive_ButtonTeam(true);
                ball.transform.LookAt(main_Camera.transform);
                ball.transform.eulerAngles = Quaternion.Euler(0, -90, 0) * ball.transform.eulerAngles + ball.transform.position;
            }

        }

        /// <summary>
        /// Unactives the button team.
        /// </summary>
        /// <param name="enabled">If set to <c>true</c> enabled.</param>
        private void setActive_ButtonTeam(bool enabled)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(enabled);
            }
        }
    }

}