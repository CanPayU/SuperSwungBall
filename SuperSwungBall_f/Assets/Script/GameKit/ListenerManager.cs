using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace GameKit {

	public class ListenerManager  {

		private static Dictionary<GameObject, List<IGameListener>> gmListeners = new Dictionary<GameObject, List<IGameListener>> ();
		private static List<IGameListener> globalListeners = new List<IGameListener> ();
		private static List<IGameListener> externalListeners = new List<IGameListener> ();

		private IGameListener listener; // me
		private bool listen = false;
		private EventType type;
		private GameObject gm;

		public ListenerManager(IGameListener listener, EventType type, GameObject gm, bool autoListen = false) {
			this.listener = listener;
			this.type = type;
			this.gm = gm;
			if (autoListen)
				StartListening ();
		}

		public void StartListening(){
			this.listen = true;
			int typeInt = (int)this.type;

			if (typeInt < 2) {
				externalListeners.Add (this.listener);
				return;
			}
			if (typeInt > 2) {
				if (!gmListeners.ContainsKey (gm))
					gmListeners [gm] = new List<IGameListener> ();
				gmListeners [gm].Add (this.listener);
			}
			if (typeInt == 2 || typeInt == 4)
				globalListeners.Add (this.listener);
		}

		public void StopListening(){
			this.listen = false;
			int typeInt = (int)this.type;

			if (typeInt < 2) {
				externalListeners.Remove (this.listener);
				return;
			}
			if (typeInt > 2) {
				if (gmListeners.ContainsKey (gm))
					gmListeners [gm].Remove (this.listener);
			}
			if (typeInt == 2 || typeInt == 4)
				globalListeners.Remove (this.listener);
		}



//		public static void AddListener(IGameListener listener, EventType type, GameObject gm) {
//
//			int typeInt = (int)type;
//
//			if (typeInt < 2) {
//				externalListeners.Add (listener);
//				return;
//			}
//			if (typeInt > 2) {
//				if (!gmListeners.ContainsKey (gm))
//					gmListeners [gm] = new List<IGameListener> ();
//				gmListeners [gm].Add (listener);
//			}
//			if (typeInt == 2 || typeInt == 4)
//				globalListeners.Add (listener);
//		}

//		public static void RemoveListener(IGameListener listener, GameObject gm, bool global = false) {
//			if (global)
//				globalListeners.Remove (listener);
//			else {
//				if (gmListeners.ContainsKey(gm))
//					gmListeners [gm].Remove (listener);
//			}
//		}

		public static List<IGameListener> getListeners(EventType type, GameObject gm = null) {

			int typeInt = (int)type;
			if (typeInt < 2) 
				return externalListeners;

			List<IGameListener> resultList = new List<IGameListener> ();
			
			if (typeInt > 2 && gm != null)
				resultList = resultList.Concat (gmListeners [gm]).ToList();
			if (typeInt == 2 || typeInt == 4)
				resultList = resultList.Concat (globalListeners).ToList();
			return resultList;
		}

		public static List<IGameListener> getListeners(GameObject gm = null, bool external = false){
			if (gm != null)
				return gmListeners [gm];
			return globalListeners;
		}


		public bool Listen {
			get { return this.listen; }
		}

	}

}