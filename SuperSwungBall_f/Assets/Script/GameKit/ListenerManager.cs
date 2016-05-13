using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameKit {

	public class ListenerManager  {

		private static Dictionary<GameObject, List<IGameListener>> gmListeners = new Dictionary<GameObject, List<IGameListener>> ();
		private static List<IGameListener> globalListeners = new List<IGameListener> ();


		public static void AddListener(IGameListener listener, EventType type, GameObject gm) {
			if ((int)type > 1)
				globalListeners.Add (listener);
			if ((int)type < 2 || (int)type == 3) {
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
	}

}