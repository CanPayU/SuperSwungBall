using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Net;
using Boomlagoon.JSON;

public static class HTTP {

	/// <summary> Nom de domaine principale  </summary>
	private const string HOST_DOMAIN = "http://ssb.shost.ca/API/";
	//private const string HOST_DOMAIN = "http://localhost:8888/SuperSwungBall/web/app_dev.php/API/";

	/// <summary> Key d'authentification  </summary>
	private const string PRIVATE_KEY = "dcbcd1627918a87ea8fc20c379c83c95";



	/// <summary>
	/// Authentifie l'utilisateur 
	/// </summary>
	/// <param name="username">Username de l'utilisateur</param>
	/// <param name="password">Password de l'utilisateur</param>
	/// <param name="completion">Fonction éxecuté lors de la réception param : <bool></param>
	public static void Authenticate(string username, string password, Action<bool> completion) 
	{
		string url = HOST_DOMAIN + "connect/" + username + "/" + password;
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		JSONObject response = execute(request);

		string status = response.GetString ("status");
		if (status == "success") {
			completion (true);
			JSONObject user = response.GetObject ("user");
			User.Instance.update (user);
		} else {
			completion (false);
		}
	}

	/// <summary>
	/// Get les informations sur le serveur
	/// Il doir être authentifié. 
	/// </summary>
	/// <param name="completion">Fonction éxecuté lors de la réception param : <bool></param>
	public static void SyncUser(Action<bool> completion) 
	{
		User user = User.Instance;
		if (!user.is_connected) {
			completion (false); 
			return;
		}

		string url = HOST_DOMAIN + "sync/" + user.username + "/" + user.id + "/" + PRIVATE_KEY;
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		JSONObject response = execute(request);

		string status = response.GetString ("status");
		if (status == "success") {
			completion (true);
			JSONObject userjson = response.GetObject ("user");
			User.Instance.update (userjson);
		} else {
			completion (false);
		}
	}


	/// <summary>
	/// Synchronise le score de l'utilisateur.
	/// Il doit être authentifié. 
	/// </summary>
	/// <param name="score_to_add">Score à ajouter</param>
	/// <param name="completion">Fonction éxecuté lors de la réception param : <bool></param>
	public static void SyncScore(int score_to_add, Action<bool> completion) 
	{
		User user = User.Instance;
		if (!user.is_connected) {
			completion (false); 
			return;
		}

		string url = HOST_DOMAIN + "unity/score/" + user.username + "/" + user.id + "/" + score_to_add;
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		JSONObject response = execute(request);

		string status = response.GetString ("status");
		if (status == "success") {
			completion (true);
			JSONObject userjson = response.GetObject ("user");
			User.Instance.update (userjson);
		} else {
			completion (false);
		}
	}


	/// <summary>
	/// Set les Phis sur le serveur
	/// Il doit être authentifié. 
	/// </summary>
	public static void SetPhi(int value, Action<bool> completion) 
	{
		User user = User.Instance;
		if (!user.is_connected) {
			completion (false); 
			return;
		}

		string url = HOST_DOMAIN + "unity/phi/" + user.username + "/" + user.id + "/" + value + "/" + PRIVATE_KEY;
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		JSONObject response = execute(request);

		string status = response.GetString ("status");
		if (status == "success") {
			JSONObject userjson = response.GetObject ("user");
			User.Instance.update (userjson);
			completion (true);
		} else {
			completion (false);
		}
	}


	/// <summary> Get les SwungMens sur le serveur  </summary>
	public static void SwungMens(Action<bool, JSONArray> completion) 
	{
		string url = HOST_DOMAIN + "unity/swungmens/" + PRIVATE_KEY;
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		JSONObject response = execute(request);

		string status = response.GetString ("status");
		if (status == "success") {
			JSONArray json = response.GetArray ("swungmens");
			completion (true, json);
		} else {
			completion (false, null);
		}
	}


	/// <summary>
	/// Execute les requêtes et renvoie le JSON
	/// </summary>
	/// <param name="request">Destination de la requête</param>
	private static JSONObject execute(HttpWebRequest request) {
		try {
			WebResponse response = request.GetResponse();
			using (Stream responseStream = response.GetResponseStream()) {
				StreamReader reader = new StreamReader(responseStream);
				return JSONObject.Parse(reader.ReadToEnd());
			}
		}
		catch (WebException ex) {
			Debug.Log (ex.Message);
			JSONObject jsonError = new JSONObject ();
			var valueError = new KeyValuePair<string, JSONValue> ("status", "error");
			jsonError.Add (valueError);
			return jsonError;
		}
	}
}