using UnityEngine;
using System.Collections;

public class Game {

	private static Game game_instance = new Game ();
	public static Game Instance {
		get{
			return game_instance;
		}
	}

	private Team[] teams;
	public Team[] Teams {
		get {
			return teams;
		}
	}

	public Game (){
		teams = new Team[2];
		teams [0] = new Team ("SuperTeam1");
		teams [1] = new Team ("SuperTeamMega2");
	}

	public void goal(int team_id){
		Team team = teams [team_id];
		team.Points = 1;
		//Debug.Log (team.Name + " :"+ team.Points);
	}
}
