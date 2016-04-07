using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;


[System.Serializable]
public sealed class User {

	private static User _instance = new User ();
	public static User Instance { 	
		get { return _instance; }
		set { _instance = value; }
	}

	public const string VERSION = "1.07"; // Version actuelle
	public string version; // Version de l'instance (sauvegarder sur l'ordi)

	public int id;
	public string username;
	public string email;
	public int score;
	public string room;
	public string[] roles;
	private Friends friends;
	public bool is_connected;

	public User() {
		is_connected = false;
		id = 0;
		username = null;
		email = null;
		score = 0;
		roles = null;
		room = null;
		friends = null;
		version = VERSION;
	}

	public void update(JSONObject json){
		is_connected = true;
		id = (int)json.GetNumber ("id");
		username = json.GetString ("username");
		email = json.GetString ("email");
		score = (int)json.GetNumber ("score");
		room = json.GetString ("room");

		if (!json.ContainsKey ("friends")) {
			this.friends = new Friends ();
			return;
		}
		
		// -- Friends
		JSONArray friends = json.GetArray ("friends");
		this.friends = new Friends (friends);
	}

	public Friends Friends{
		get { return friends; }
	}
}