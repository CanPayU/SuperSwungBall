using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public sealed class User {

	private static User _instance = new User ();
	public static User Instance
	{
		get
		{
			return _instance;
		}
	}

	public int id;
	public string username;
	public string email;
	public int score;
	public string[] roles;
	public bool is_connected;

	public User() {
		id = 0;
		username = null;
		email = null;
		score = 0;
		roles = null;
		is_connected = false;
	}

	public void update(JSONObject json){
		id = (int)json.GetNumber ("id");
		username = json.GetString ("username");
		email = json.GetString ("email");
		score = (int)json.GetNumber ("score");
		is_connected = true;
	}
}
