using UnityEngine;
using System.Collections;

using GameKit;

namespace GameScene
{
	public class CameraController : GameBehavior
    {

        private Vector3 POSITION_INITIALE = new Vector3(25, 35, 0); // constante position phase de reflexion

        private new bool animation;

        // mouvement camera
        private float speed;
        GameObject ball; // pour suivre le mouvement de la balle

		public CameraController(){
			this.eventType = GameKit.EventType.Global;
		}
			
        void Start()
        {
            speed = 10;
            ball = GameObject.Find("Ball");
            animation = false;
        }

        void Update()
        {
            if (animation)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(ball.transform.position.x + 15, 10, ball.transform.position.z), Time.deltaTime * speed);
                transform.LookAt(ball.transform.position);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, POSITION_INITIALE, Time.deltaTime * speed);
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

		public override void OnStartAnimation(){
			this.animation = true;
			this.speed = 1;
			this.transform.position = new Vector3(ball.transform.position.x + 15, 10, ball.transform.position.z);
		}
		public override void OnStartReflexion(){
			this.animation = false;
			this.speed = 20;
		}
    }
}