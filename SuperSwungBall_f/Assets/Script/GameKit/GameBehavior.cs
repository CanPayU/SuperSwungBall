using UnityEngine;
using System.Collections;

using GameScene;

namespace GameKit {

	public class GameBehavior : MonoBehaviour, IGameListener {

		/// <summary> Permet d'appeler un évennement. </summary>
		internal Call Caller = null;

		private bool isListener = false;

		internal EventType eventType = EventType.Local;

		void Awake(){
			this.Caller = new GameBehavior.Call(this);
			ListenerManager.AddListener (this, eventType, gameObject);
			isListener = true;
		}

		void OnDisable(){
			if (isListener) {
				ListenerManager.AddListener (this, eventType, gameObject);
				isListener = false;
			}
		}

		void OnEnable() {
			if (!isListener) {
				ListenerManager.AddListener (this, eventType, gameObject);
				isListener = true;
			}
		}

		///
		/// Event
		///

		public virtual void OnStartAnimation() { }
		public virtual void OnStartReflexion() { }
		public virtual void OnGoal(GoalController g) { }

		public virtual void OnSucceedAttack (Player pl) { }
		public virtual void OnSucceedEsquive (Player pl) { }
		public virtual void OnFailedAttack (Player pl) { }
		public virtual void OnFailedEsquive (Player pl) { }


		///
		/// Call Event
		///

		internal class Call {

			private GameBehavior parent;

			public Call(GameBehavior parent) {
				this.parent = parent;
			}

			internal void StartAnimation() {
				callGlobalListeners ("OnStartAnimation", null);
			}

			internal void StartReflexion() {
				callGlobalListeners ("OnStartReflexion", null);
			}
		
			internal void Goal(GoalController g) {
				callGlobalListeners ("OnGoal", new object[] { g });
			}

			internal void SuccessEsquive(Player p) {
				callListeners ("OnSuccessEsquive", new object[] { p });
			}

			internal void SuccessAttack(Player p) {
				callListeners ("OnSuccessAttack", new object[] { p });
			}

			internal void FailedAttack(Player p) {
				callListeners ("OnFailedAttack", new object[] { p });
			}

			internal void FailedEsquive(Player p) {
				callListeners ("OnFailedEsquive", new object[] { p });
			}

			private void callListeners(string methodName, object[] parameters){
				foreach (var l in ListenerManager.getListeners(parent.gameObject)) {
					typeof(IGameListener).GetMethod(methodName).Invoke(l, parameters);
				}
			}

			private void callGlobalListeners(string methodName, object[] parameters){
				foreach (var l in ListenerManager.getListeners()) {
					typeof(IGameListener).GetMethod(methodName).Invoke(l, parameters);
				}
			}

//			private void callListeners(string methodName, object[] parameters){
//				foreach (var l in ListenerManager.listeners) {
//					typeof(IGameListener).GetMethod(methodName).Invoke(l, parameters);
//				}
//			}
		}
	
	}

}