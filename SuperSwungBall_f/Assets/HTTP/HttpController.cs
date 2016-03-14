using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using Boomlagoon.JSON;

public class HttpController: MonoBehaviour {
	
	private string host_domain = "http://ssb.shost.ca/API/";
	//private string host_domain = "http://localhost:8888/SuperSwungBall/web/app_dev.php/API/";

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
	}

	public void connect(string username, string password, Action<bool> completion){
		string url = host_domain + "connect/" + username + "/" + password;
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
	}

	IEnumerator WaitForRequest(WWW www, Action<JSONObject> completion)
	{
		yield return www;
		// check for errors
		if (www.error == null)
		{
			var data = www.text;
			var json = JSONObject.Parse (data);

			completion (json);
		} else {
			Debug.Log("WWW Error: "+ www.error);
			completion(null);
		}  
	}
}
