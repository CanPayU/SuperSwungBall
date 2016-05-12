using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameKit {

	public class ListenerManager  {

		private static Dictionary<GameObject, List<IGameListener>> gmListeners = new Dictionary<GameObject, List<IGameListener>> ();
		private static List<IGameListener> globalListeners = new List<IGameListener> ();

		public static void AddListener(IGameListener listener, GameObject gm, bool global = false) {
			if (global)
				globalListeners.Add (listener);
			else {
				if (!gmListeners.ContainsKey(gm))
					gmListeners [gm] = new List<IGameListener> ();
				gmListeners [gm].Add (listener);
			}
		}
		public static void RemoveListener(IGameListener listener, GameObject gm, bool global = false) {
			if (global)
				globalListeners.Remove (listener);
			else {
				if (gmListeners.ContainsKey(gm))
					gmListeners [gm].Remove (listener);
			}
		}

		public static List<IGameListener> getListeners(GameObject gm = null){
			if (gm != null)
				return gmListeners [gm];
			return globalListeners;
		}

		// ---- V1

//		public static List<IGameListener> listeners = new List<IGameListener>();
//
//		public static void AddListener(IGameListener listener) {
//			listeners.Add (listener);
//			//Debug.Log ("AddListener");
//		}
//
//		public static void RemoveListener(IGameListener listener) {
//			listeners.Remove (listener);
//			//Debug.Log ("RemoveListener");
//		}

	}

}