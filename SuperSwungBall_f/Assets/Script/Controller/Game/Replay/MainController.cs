using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using GameKit;

namespace GameScene.Replay {

	public class MainController : GameScene.BasicMainController {

		private Replay replay;

		public MainController(){
			this.eventType = GameKit.EventType.Global;
		}

		// Use this for initialization
		protected override void Start()
		{
			this.playerController = typeof(GameScene.Replay.PlayerController);
			this.replay = SaveLoad.load_replay ();
			this.replay.resetActualValue ();
			Game.Instance = this.replay.Game;
			base.Start ();
		}

		protected override void end_time()
		{
			this.time.reset();
			Caller.StartAnimation();
		}

		// ------- Event
		public override void OnStartAnimation(){
			this.replay.nextRound ();
			base.OnStartAnimation ();
		}
		public override void OnStartReflexion(){}


		public PlayerAction getPlayerAction(Player player) {
			return this.replay.ActualRound.getPlayerAction (player);
		}
	}
}