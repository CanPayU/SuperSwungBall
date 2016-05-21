using UnityEngine;
using System.Collections.Generic;

using GameKit;

namespace GameScene.Solo
{
	public class PlayerController : BasicPlayerController
	{
		public PlayerController(){
			this.eventType = GameKit.EventType.All;
		}

		// Rien de spécifique au mode solo ...
	}
}