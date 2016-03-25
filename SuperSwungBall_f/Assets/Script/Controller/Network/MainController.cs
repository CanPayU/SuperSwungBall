using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;

namespace Network {
	public class MainController : MonoBehaviour {
		
		[SerializeField] private GameObject connection_panel;
		[SerializeField] private GameObject loading_panel;
		[SerializeField] private GameObject private_panel;

		// Use this for initialization
		void Start () {

			#if DEBUG
				SaveLoad.load_user ();
			#endif


			Debug.Log(HttpController.GET("antoine","mdp"));



			if (!User.Instance.is_connected) { 
				//connection();
				connection_panel.SetActive (true);
			} else {
				//SaveLoad.load_settings (); // a supprimer
				loading_panel.SetActive (true);
			}
		}

		public void private_game(){
			PhotonNetwork.Disconnect ();
			private_panel.SetActive (true);
			loading_panel.SetActive (false);
			connection_panel.SetActive (false);
		}


		public void connect_network(){
			loading_panel.SetActive (true);
			private_panel.SetActive (false);
			connection_panel.SetActive (false);
		}
		/*
		public void sync_score(int score_to_add, Action<bool> completion){
			User user = User.Instance;

			if (!user.is_connected) {
				completion (false); return;
			}


			string url = host_domain + "unity/score/" + user.username + "/" + user.id + "/" + score_to_add;
			WWW www = new WWW(url);
			StartCoroutine(WaitForRequest(www, (json)=>{
				if (json == null) {
					completion(false); return;
				}
				var status = json.GetString ("status");
				if(status != "success"){
					completion(false); return;
				}
				User.Instance.update(json.GetObject("user"));
				completion(true);
			}));
		}*/

	}
}
