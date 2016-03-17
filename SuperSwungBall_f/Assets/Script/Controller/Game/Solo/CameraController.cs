using UnityEngine;
using System.Collections;

namespace GameScene.Solo
{
    public class CameraController : MonoBehaviour
    {

        private Vector3 POSITION_INITIALE = new Vector3(15, 40, 0); // constante position phase de reflexion

        private new bool animation;

        // mouvement camera
        private Vector3 moveTo;
        private Vector3 lookAt;
        private float speed;
        GameObject ball; // pour suivre le mouvement de la balle

        void Start()
        {
            moveTo = POSITION_INITIALE;
            lookAt = new Vector3(0, 0, 0);
            speed = 10;
            ball = GameObject.Find("Ball");
            animation = false;
        }

        void Update()
        {
            if(animation)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(ball.transform.position.x + 15, 10, ball.transform.position.z), Time.deltaTime * speed);
                transform.LookAt(ball.transform.position);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position,POSITION_INITIALE, Time.deltaTime * speed);
                transform.LookAt(Vector3.zero);
            }
        }

        public void start_anim()
        {
            animation = true;
            speed = 1;
            transform.position = new Vector3(ball.transform.position.x + 15, 10, ball.transform.position.z);
        }
        public void end_anim()
        {
            animation = false;
            speed = 20;
        }
    }
}