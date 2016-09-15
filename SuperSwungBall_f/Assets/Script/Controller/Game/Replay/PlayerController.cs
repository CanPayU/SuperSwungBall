using UnityEngine;
using System.Collections;


namespace GameScene.Replay
{

    public class PlayerController : GameScene.BasicPlayerController
    {

        private MainController mainController;

        private bool makePass;
        private Vector3 posPass;

        public PlayerController()
        {
            this.eventType = GameKit.EventType.All;
        }

        protected override void Start()
        {
            base.Start();
            this.isMine = false;
            this.mainController = GameObject.Find("Main").GetComponent<MainController>();
        }

        protected override void Update()
        {
            base.Update();

            //if (phaseAnimation)
            //{
            //	if (transform.position == this.posPass)
            //		Debug.Log("PASSE OK");
            //	if (Input.GetKeyDown(KeyCode.X) && this.player.Name == "Epiteen-3-3")
            //	{
            //		Debug.Log(this.makePass + " -- " + transform.position);
            //		GameObject.Find("Ball").GetComponent<BallController>().ExecutePasse();
            //	}

            //}
            var dist = Vector3.Distance(transform.position, this.posPass);
            if (phaseAnimation && this.makePass && dist < 0.1)
            {
                Debug.Log("Fait la passe");
                GameObject.Find("Ball").GetComponent<BallController>().ExecutePasse();
                this.makePass = false; // Passe faite
            }
        }

        // -- Event
        public override void OnStartAnimation()
        {
            PlayerAction action = this.mainController.getPlayerAction(this.player);
            setMyParam(action);
            start_Anim(false);
        }
        public override void OnStartReflexion()
        {
            end_Anim();
        }

        private void setMyParam(PlayerAction action)
        {
            this.PointDeplacement = action.Deplacement;
            this.Player.Button_Values = action.ButtonValues;

            if (action.MakePasse)
            {
                this.makePass = true;
                this.PointPasse = action.Passe;
                this.posPass = action.PosPasse;
            }
            else
                this.makePass = false;
        }
    }
}