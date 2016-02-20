using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game {

	private static Game game_instance = new Game ();
	public static Game Instance {
		get{
			return game_instance;
		}
	}

	private Dictionary<int, Team> teams;
	public Dictionary<int, Team> Teams {
		get {
			return teams;
		}
	}

	public Game (){
		teams = new Dictionary<int, Team>();
		Debug.Log ("MyID :" + PhotonNetwork.player.ID);
		foreach (PhotonPlayer player in PhotonNetwork.playerList) {
			Debug.Log ("New team :" + player.ID);
			teams [player.ID] = new Team (player.name);
		}
		//teams [0] = new Team ("SuperTeam1");
		//teams [1] = new Team ("SuperTeamMega2");
	}

	public void goal(int team_id){
		Team team = teams [team_id];
		team.Points = 1;
		//Debug.Log (team.Name + " :"+ team.Points);
	}
}
