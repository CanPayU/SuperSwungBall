using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameScene.Solo;
using GameScene.Multi;

using GameScene;

public class Game {

	private static Game game_instance = new Game ();

	private Dictionary<int, Team> teams;
	private bool finished;
	private int max_point = 3;

	public Game (){
		finished = false;
		teams = new Dictionary<int, Team>();
		if (PhotonNetwork.inRoom) {
			foreach (PhotonPlayer player in PhotonNetwork.playerList) {
				teams [player.ID] = Settings.Instance.Selected_Team; // Ancien
				//teams [player.ID].Name = player.name;
			}
		} else {
			Debug.Log ("Mach : " + Settings.Instance.Selected_Team.Name + " VS " + Settings.Instance.Random_Team.Name);
			teams [0] = Settings.Instance.Selected_Team;
			teams [1] = Settings.Instance.Random_Team;
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
		if (PhotonNetwork.inRoom) {
			main.GetComponent<MainController> ().update_score ();
			main.GetComponent<EndController> ().on_end (End.TIME);
		} else {
			main.GetComponent<Main_Controller> ().update_score ();
		}
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
