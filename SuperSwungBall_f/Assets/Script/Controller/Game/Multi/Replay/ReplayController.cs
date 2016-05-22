using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using GameKit;
using GameScene.Replay;

namespace GameScene.Multi.Replay
{
	using Replay = GameScene.Replay.Replay;
	/// <summary>
	/// Enregistre et synchronise les actions réalisé pendant la game.
	/// </summary>
	public class ReplayController : GameBehavior
	{
		private Replay replay;

		public ReplayController(){
			this.eventType = GameKit.EventType.Global;
		}

		void Start(){
			var myTeamPlayers = Game.Instance.MyTeam.Players.ToList ();
			var ennemyTeamPlayers = Game.Instance.EnnemyTeam.Players.ToList ();
			var players = myTeamPlayers.Concat (ennemyTeamPlayers).ToList();
			this.replay = new Replay (players);
		}

		void Update(){
			if (Input.GetKeyDown (KeyCode.K)) 
				Debug.Log (replay);
			if (Input.GetKeyDown (KeyCode.J))
				SaveLoad.save_replay (replay);
		}

		public override void OnStartAnimation(){
			this.replay.newRound ();
		}
		public override void OnEndGame(End type) {
			SaveLoad.save_replay (replay);
		}

		public void setPlayerAction(Player player, PlayerAction action) {
			this.replay.ActualRound.setPlayerAction (player, action);
		}
	}
}