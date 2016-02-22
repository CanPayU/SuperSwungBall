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

	public Settings (){
		default_team = new Dictionary<string, Team> ();

		string[] def_sound= new string[] { "Musics/Team/PSG/Allez Paris [classic]" };
		//AudioClip a = Resources.Load ("Musics/Team/PSG/Allez Paris [classic]") as AudioClip;
		//def_sound.Add ("Musics/Team/PSG/Allez Paris [classic]");
		default_team.Add("psg", new Team("PSG", def_sound));


		default_team.Add("france", new Team("France", def_sound));
	}

	public Dictionary<string, Team> Default_Team {
		get { return default_team; }
	}
}
