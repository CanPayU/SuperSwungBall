using UnityEngine;
using System.Collections;

namespace View {

	public class ViewController : MonoBehaviour {

		// Use this for initialization
		void Start () {
			ApplicationModel.AddSubview ();
		}
		
		// Update is called once per frame
		void OnDisable () {
			ApplicationModel.RemoveSubview ();
		}
	}
}