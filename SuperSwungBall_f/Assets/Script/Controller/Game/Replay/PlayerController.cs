using UnityEngine;
using System.Collections;


namespace GameScene.Replay {

	public class PlayerController : GameScene.BasicPlayerController {

		private MainController mainController;

		public PlayerController(){
			this.eventType = GameKit.EventType.All;
		}

		protected override void Start()
		{
			base.Start ();
			this.isMine = false;
			this.mainController = GameObject.Find ("Main").GetComponent<MainController> ();
		}
			

		// -- Event
		public override void OnStartAnimation(){
			PlayerAction action = this.mainController.getPlayerAction (this.player);
			setMyParam(action);
			start_Anim (false);
		}
		public override void OnStartReflexion(){
			end_Anim();
		}

		private void setMyParam(PlayerAction action)
		{
			this.PointDeplacement = action.Deplacement;
			this.PointPasse = action.Passe;
			this.Player.Button_Values = action.ButtonValues;
		}
	}
}