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

	/// <summary> Ajouter un ami </summary>
	public void Add(string username, User user){
		friends.Add(username, user);
	}

	/// <summary> Supprime un ami </summary>
	public void Remove(string username){
		friends.Remove (username);
	}

	/// <summary> Contient l'ami ... ? </summary>
	public bool Contains(string username){
		return friends.ContainsKey (username);
	}

	/// <summary> Renvoi l'ami ... </summary>
	public User Get(string username){
		return friends[username];
	}

	/// <summary> Set la room de l'ami </summary>
	public void SetRoom(string username, string roomID){
		if (!friends.ContainsKey (username))
			return;
		User user = friends [username];
		user.room = roomID;
	}

	/// <summary> Set si l'ami ... est en ligne ou non </summary>
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
