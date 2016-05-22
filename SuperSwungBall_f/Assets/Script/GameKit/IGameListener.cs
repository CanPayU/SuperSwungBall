using UnityEngine;
using System.Collections;

using GameScene;

namespace GameKit {

	public interface IGameListener {


		// ============== GlobalEvent


		/// <summary> Appelé lorsque la phase d'annimation débute. </summary>
		void OnStartAnimation();

		/// <summary> Appelé lorsque la phase de réfléxion débute. </summary>
		void OnStartReflexion();

		/// <summary>
		/// Appelé lorsqu'un but est marqué.
		/// </summary>
		/// <param name="goal">GoalController du gameObject</param>
		void OnGoal(GoalController goal);

		/// <summary> Appelé lors de la fin de la game. </summary>
		void OnEndGame(GameScene.Multi.End type);


		// ============== LocalEvent


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