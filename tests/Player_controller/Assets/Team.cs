using System.Collections;
using  System.Collections.Generic;
using UnityEngine;
using Assets;

public class Team {
	
	private List<Player_controller> players;
	private string name;
	private int points;

	private int nb_player;


	public Team(string name_){
		name = name_;
		points = 0;
		nb_player = 5;
		players = new List<Player_controller>();
	}

	public void start_move_players(){
		foreach (Player_controller player in players) {
			player.start_Anim();
		}
	}
	public void end_move_players(){
		foreach (Player_controller player in players) {
			player.end_Anim();
		}
	}

	public bool add_player(GameObject prefab){
		Player_controller player = prefab.GetComponent<Player_controller> ();
		if (players.Count < nb_player) {
			players.Add (player);
			return true;
		}
		return false;
	}

	public int Points {
		get {
			return points;
		}
		set {
			points += value;
		}
	}
	public string Name {
		get {
			return name;
		}
	}
	public int Nb_Player {
		get {
			return nb_player;
		}
	}
}
