using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameScene;

public class Game {

	private static Game game_instance = new Game ();

	private Dictionary<int, Team> teams;
	private bool finished;
	private int max_point = 1;

	public Game (){
		finished = false;
		teams = new Dictionary<int, Team>();
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			teams [player.ID] = new Team (player.name);
		}
	}

	public void goal(int team_id){
		Team team = teams [team_id];
		team.Points = 1;
		if (team.Points >= max_point) {
			finished = true;
			end_game ();
		}
	}

	private void end_game(){
		GameObject main = GameObject.Find ("Main");
		main.GetComponent<MainController> ().update_score ();
		main.GetComponent<EndController> ().on_end (End.TIME);
	}

	public static Game Instance {
		get{
			return game_instance;
		}
		set {
			game_instance = value;
		}
	}
	public Dictionary<int, Team> Teams {
		get {
			return teams;
		}
	}
	public bool isFinish {
		get {
			return finished;
		}
		set {
			finished = value;
		}
	}
}
