using UnityEngine;
using System.Collections;

using GameScene;

namespace GameKit {

	public interface IGameListener {

		/// <summary>
		/// Appelé lorsqu'un but est marqué.
		/// </summary>
		/// <param name="goal">GoalController du gameObject</param>
		void OnGoal(GoalController goal);

		/// <summary>
		/// Appelé lorsque le player résussi son attaque.
		/// </summary>
		/// <param name="p">Player concerné</param>
		void OnSucceedAttack (Player p);

		/// <summary>
		/// Appelé lorsque le player résussi son esquive.
		/// </summary>
		/// <param name="p">Player concerné</param>
		void OnSucceedEsquive (Player p);

		/// <summary>
		/// Appelé lorsque le player rate son attaque.
		/// </summary>
		/// <param name="p">Player concerné</param>
		void OnFailedAttack (Player p);

		/// <summary>
		/// Appelé lorsque le player rate son esquive.
		/// </summary>
		/// <param name="p">Player concerné</param>
		void OnFailedEsquive (Player p);
	}

}