using System.Collections;
using  System.Collections.Generic;
using UnityEngine;
using Assets;
using System.Reflection;

public class Team {
	
	private List<Player> players;
	private string name;
	private int points;

	private int nb_player;


	public Team(string name_){
		name = name_;
		points = 0;
		nb_player = 5;
		players = new List<Player>();
	}

	public void start_move_players(){
		foreach (Player player in players) {
			Player_controller controller = player.Gm.GetComponent<Player_controller>();
			controller.start_Anim();
		}
	}
	public void end_move_players(){
		foreach (Player player in players) {
			Player_controller controller = player.Gm.GetComponent<Player_controller>();
			controller.end_Anim();
		}
	}

	public bool add_player(Player player){
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
