using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;


namespace Gestion {
	
	public class MainController : MonoBehaviour {

		private SlideController slide;

		// Use this for initialization
		void Start () {
			this.slide = GameObject.Find ("ScrollView").GetComponent<SlideController> ();

			SaveLoad.load_user ();
			SaveLoad.load_settings ();


			HTTP.SwungMens ((success, json) => {
				if (success)
					Instanciate(json);
				else
					Debug.LogError("Impossible de charger les SungMens");
			});
		}

		private void Instanciate(JSONArray json){
			foreach (JSONValue swungMen in json) {
				Player player = new Player (swungMen.Obj);
				Settings.Instance.AddOrUpdate_PaidPlayer (player);
				slide.InstanciatePlayer (player);
			}
		}
	}
}