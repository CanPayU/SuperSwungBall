using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Boomlagoon.JSON;

[System.Serializable]
public class Friends {

	private Dictionary<string,User> friends;

	public Friends() {
		this.friends = new Dictionary<string,User> ();
	}

	public Friends(JSONArray array) {
		this.friends = new Dictionary<string,User> ();//new User[friends.Length];
		foreach (JSONValue friend in array) {
			User friend_user = new User ();
			friend_user.update (friend.Obj);
			friend_user.is_connected = false;
			this.friends.Add (friend_user.username, friend_user);
		}
	}

	public void Add(string username, User user){
		friends.Add(username, user);
	}
	public void Remove(string username){
		friends.Remove (username);
	}
	public bool Contains(string username){
		return friends.ContainsKey (username);
	}
	public User Get(string username){
		return friends[username];
	}
	public void IsOnline(string username, bool online = true){
		if (!friends.ContainsKey (username))
			return;
		User user = friends [username];
		user.is_connected = online;
	}

	public Dictionary<string,User> All{
		get { return friends; }
	}
}
