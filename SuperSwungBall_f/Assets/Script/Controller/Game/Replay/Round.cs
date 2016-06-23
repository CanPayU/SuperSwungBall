using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace GameScene.Replay
{
	/// <summary>
	/// Type contenant toutes les actions réalisées durant le round
	/// </summary>
	[System.Serializable]
	public class Round
	{
		/// <summary>
		/// Identifiant du round.
		/// </summary>
		public int id;

		private Dictionary<Player, PlayerAction> actions;

		public Round (int id, List<Player> players) {
			this.id = id;
			this.actions = new Dictionary<Player, PlayerAction> ();
			initializeActions (players);
		}

		public void setPlayerAction(Player player, PlayerAction action) {
			actions [player] = actions[player].UpdateWith (action);
		}

		public PlayerAction getPlayerAction(Player player) {
			Debug.LogWarning ("Prendre en charge le cas ou le player n'héxiste pas");
			return actions [player];
		}

		public override string ToString ()
		{
			string str = "[Round: id=" + id + ",  countActions="+ actions.Count +", actions=[\n";
			foreach (var action in actions) {
				str += string.Format("   [{0}, {1}]\n", action.Key.Name, action.Value);
			}
			str += "]\n]";
			return str;
		}

		private void initializeActions(List<Player> players){
			int i = 1;
			foreach (Player player in players) {
				int idAction = this.id * 1000 + i;
				PlayerAction pAction = new PlayerAction (idAction, Vector3.zero, Vector3.zero, new List<string> ());
				actions [player] = pAction;
			}
		}
	}
}