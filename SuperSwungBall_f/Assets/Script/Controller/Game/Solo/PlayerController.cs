using UnityEngine;
using System.Collections.Generic;

using GameKit;
using GameScene.Multi.Replay;
using GameScene.Replay;

namespace GameScene.Solo
{
	public class PlayerController : BasicPlayerController
	{
		public PlayerController(){
			this.eventType = GameKit.EventType.All;
		}


		// == Pour le DEV

		//private ReplayController replayController;

		//protected override void Start()
		//{
		//	base.Start();
		//	this.replayController = GameObject.Find("Main").GetComponent<ReplayController>();
		//}

		//public override void OnStartAnimation()
		//{
		//	PlayerAction action = new PlayerAction(0, this.PointDeplacement, this.PointPasse, this.Player.Button_Values);
		//	this.replayController.setPlayerAction(this.Player, action);
		//	base.OnStartAnimation();
		//}

		// ==

		// Rien de spécifique au mode solo ...
	}
}