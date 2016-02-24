using System.Collections;
using  System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using GameScene;

[System.Serializable]
public class Team {
	
	private List<Player> players;
	private string[] sounds;
	private string name;
	private int points;
	private string code;

	private int nb_player;


	public Team(string name_, string[] sounds_ = null, string code_ = null){
		name = name_;
		code = code_;
		sounds = (sounds_ != null) ? sounds_ : new string[0];
		points = 0;
		nb_player = 5;
		players = new List<Player>();
	}

	public void start_move_players(){
		foreach (Player player in players) {
			PlayerController controller = player.Gm.GetComponent<PlayerController>();
			controller.start_Anim();
		}
	}
	public void end_move_players(){
		foreach (Player player in players) {
			PlayerController controller = player.Gm.GetComponent<PlayerController>();
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
		get { return points; }
		set { points += value; }
	}
	public string Name {
		get { return name; }
	}
	public int Nb_Player {
		get { return nb_player; }
	}
	public string[] Sounds {
		get { return sounds; }
	}
	public string Code {
		get { return code; }
	}
	public Player[] Players {
		get {
			Player[] player_array = new Player[players.Count];
			int i = 0;
			foreach (Player player in players) {
				player_array [i] = player;
				i++;
			}
			return player_array;
		}
		set {
			List<Player> ls_p = new List<Player> ();
			foreach (Player p in value) {
				ls_p.Add (p);
			}
			players = ls_p;
		}
	}
}
