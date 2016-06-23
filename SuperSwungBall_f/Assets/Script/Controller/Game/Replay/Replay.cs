using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace GameScene.Replay
{
	/// <summary>
	/// Type contenant toutes les actions réalisées durant le round
	/// </summary>
	[System.Serializable]
	public class Replay
	{
		private List<Round> rounds;
		private List<Player> players;
		private Game game;

		private int actualId = 0;
		private Round actualRound;

		public Replay(List<Player> players){
			this.players = players;
			this.rounds = new List<Round> ();
			this.game = Game.Instance;
		}

		public void newRound(){
			this.actualId++;
			this.actualRound = new Round (this.actualId, players);
			this.rounds.Add (actualRound);
		}

		public void setPlayerAction(Player player, PlayerAction action) {
			this.actualRound.setPlayerAction (player, action);
		}


		// FOR EXECUTE
		public void nextRound(){
			this.actualId++;
			this.actualRound = this.rounds [actualId-1];
			Debug.Log(this.rounds.Count + " - " + this.actualId + " / " + this.actualRound.id);
		}

		/// <summary>
		/// Reset les valeurs temporelle
		/// </summary>
		public void resetActualValue(){
			this.actualId = 0;
			this.actualRound = null;
		}

		public override string ToString ()
		{
			string str = "[Replay: roundsCount=" + rounds.Count + ", \n";
			foreach (var round in rounds) {
				str += round;
			}
			str += "\n]";
			return str;
		}

		public Round ActualRound {
			get { return this.actualRound; }
		}
		public Game Game {
			get { return this.game; }
		}
	}
}
