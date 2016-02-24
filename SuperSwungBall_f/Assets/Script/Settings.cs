using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Settings {
	
	private static Settings _instance = new Settings ();
	public static Settings Instance {
		get { return _instance; }
		set { _instance = value; }
	}

	private Dictionary<string, Team> default_team;
	private Dictionary<string, Player> default_player;

	public Settings (){
		default_team = new Dictionary<string, Team> ();
		default_player = new Dictionary<string, Player> ();

		// ----- Default Player
		Player lombrix = new Player (4, 6, 7, 1, "Lombrix", 0);
		Player itec = new Player (1, 4, 2, 9, "Itectori", 0);
		Player gpdn = new Player (7, 4, 5, 5, "GPasDNom", 0);
		Player pwc = new Player (3, 2, 9, 2, "PlayWithCube", 0);
		Player ept = new Player (1, 1, 1, 1, "Epitechien", 0);
		default_player.Add ("lombrix", lombrix);
		default_player.Add ("itec", itec);
		default_player.Add ("gpdn", gpdn);
		default_player.Add ("pwc", pwc);
		default_player.Add ("ept", ept);
		// --------------------

		// ----- Default Team
		string[] def_sound= new string[] { "Musics/Team/PSG/Allez Paris [classic]" };
		Team psg = new Team ("PSG", def_sound, "psg");
		psg.add_player(lombrix);
		psg.add_player(gpdn);
		default_team.Add("psg", psg);

		Team fr = new Team ("France", def_sound, "fr");
		fr.add_player(itec);
		fr.add_player(pwc);
		fr.add_player(ept);
		default_team.Add("fr", fr);
		// ------------------
	}

	public void AddOrUpdate_Team(Team t){
		if (default_team.ContainsKey (t.Code))
			default_team [t.Code] = t;
		else
			default_team.Add (t.Code, t);
	}

	public Dictionary<string, Team> Default_Team {
		get { return default_team; }
	}
	public Dictionary<string, Player> Default_player {
		get { return default_player; }
	}
}
