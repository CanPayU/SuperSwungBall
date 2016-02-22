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

	private int nb_player;


	public Team(string name_, string[] sounds_ = null){
		name = name_;
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
}
