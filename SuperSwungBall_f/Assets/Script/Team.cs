using System.Collections;
using  System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using GameScene;

[System.Serializable]
public class Team {
	
	private Dictionary<int,Player> players;
	private string[] sounds;
	private string name;
	private int points;
	private string code;

	private int nb_player;

	private Composition compo;

	public Team(string name_, Composition compo_ = null, string[] sounds_ = null, string code_ = null){
		name = name_;
		code = code_;
		sounds = (sounds_ != null) ? sounds_ : new string[0];
		compo = (compo_ != null) ? compo_ : new Composition("_");
		points = 0;
		nb_player = 5;
		players = new Dictionary<int,Player>();
	}

	public void start_move_players(){
		foreach (KeyValuePair<int,Player> player in players) {
			PlayerController controller = player.Value.Gm.GetComponent<PlayerController>();
			controller.start_Anim();
		}
	}
	public void end_move_players(){
		foreach (KeyValuePair<int,Player> player in players) {
			PlayerController controller = player.Value.Gm.GetComponent<PlayerController>();
			controller.end_Anim();
		}
	}

	public bool add_player(Player player){
		int count = players.Count;
		if (count < nb_player) {
			players.Add (count, player);
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
	public Composition Compo {
		get { return compo; }
		set { compo = value; }
	}

	public Player[] Players {
		get {
			Player[] player_array = new Player[players.Count];
			foreach (KeyValuePair<int,Player> player in players) {
				player_array [player.Key] = player.Value;
			}
			return player_array;
		}
		set {
			Dictionary<int,Player> dict_p = new Dictionary<int,Player> ();
			int i = 0;
			foreach (Player p in value) {
				dict_p.Add (i,p); i++;
			}
			players = dict_p;
		}
	}
}