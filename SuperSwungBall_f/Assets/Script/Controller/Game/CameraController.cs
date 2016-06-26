using UnityEngine;
using System.Collections;

using GameKit;

namespace GameScene
{
	public class CameraController : GameBehavior
    {

        private Vector3 POSITION_INITIALE = new Vector3(20, 35, 0); // constante position phase de reflexion
        private Vector3 posRelative = new Vector3();

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
                transform.position = Vector3.Lerp(transform.position, new Vector3(ball.transform.position.x, 0, ball.transform.position.z) + posRelative, Time.deltaTime * speed);
                transform.LookAt(ball.transform.position);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, POSITION_INITIALE, Time.deltaTime * speed);
                transform.LookAt(new Vector3(3,0,0));
            }
        }

        public void start_anim()
        {
            animation = true;
            speed = 1;
            posRelative = new Vector3(12, 5, 0);
            transform.position = new Vector3(ball.transform.position.x, 0, ball.transform.position.z) + posRelative;
        }
        public void end_anim()
        {
            animation = false;
            speed = 20;
        }

		public override void OnStartAnimation(){
			this.animation = true;
			this.speed = 1;
            posRelative = new Vector3(12, 5, 0);
            transform.position = new Vector3(ball.transform.position.x, 0, ball.transform.position.z) + posRelative;
        }
		public override void OnStartReflexion(){
			this.animation = false;
			this.speed = 20;
		}
        public override void OnGoal(GoalController goal)
        {
            speed = 3;
            posRelative = new Vector3(3, 3, 3);
            animation = true;
            transform.position = new Vector3(ball.transform.position.x, 0, ball.transform.position.z) + posRelative;
        }
    }
}