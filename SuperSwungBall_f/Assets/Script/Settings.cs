using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class Settings
{

    private static Settings _instance = new Settings();
    public static Settings Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public const string VERSION = "1.20"; // Version actuelle
    public string version; // Version de l'instance (sauvegarder sur l'ordi)

    private Dictionary<string, Team> default_team;
    private Dictionary<string, Player> default_player;
    private Dictionary<string, Composition> default_compo;

    private Team selected_team;
    private string selected_stadium_name;
    private System.Random rand = new System.Random();

	private Dictionary<string, Player> paid_player;
	private Dictionary<string, Player> secret_player; // in Chest

    // -- Keyboard
    private Dictionary<KeyboardAction, KeyCode> keyboard;
    // --

    private NotificationState notificationState;

    public Settings()
    {
        this.version = VERSION;
        this.notificationState = NotificationState.All;
        this.keyboard = new Dictionary<KeyboardAction, KeyCode>();
		this.paid_player = new Dictionary<string, Player>();
		this.secret_player = new Dictionary<string, Player>();

        // -- Setup Keyboard
        this.keyboard.Add(KeyboardAction.Passe, KeyCode.A);
        // --

		ResetDefaultPlayer ();
        this.default_team = new Dictionary<string, Team>();
        this.default_compo = new Dictionary<string, Composition>();
        this.selected_stadium_name = "Stadium_0";


        // ----- Default Compo
        Composition compo_psg = new Composition("PSG", "psg");
        compo_psg.SetPosition(0, 0, 0);
        compo_psg.SetPosition(1, 1, 1);
        compo_psg.SetPosition(2, 2, 2);
        compo_psg.SetPosition(3, 3, 3);
        compo_psg.SetPosition(4, 5, 5);
        Default_compo.Add("psg", compo_psg);

        Composition compo_fr = new Composition("FRANCE", "fr");
        compo_fr.SetPosition(0, 3, 3);
        compo_fr.SetPosition(1, 1, 3);
        compo_fr.SetPosition(2, 2, 5);
        compo_fr.SetPosition(3, 1, 2);
        compo_fr.SetPosition(4, 5, 4);
        Default_compo.Add("fr", compo_fr);
        // -------------------

        // ----- Default Team FOR DEBUG
        string[] def_sound = new string[] { "Musics/Team/PSG/Allez Paris [classic]" };
        Team psg = new Team("PSG", compo_psg, def_sound, "psg");
        psg.add_player(new Player(4, 6, 7, 1, "Lombrix", null));
        psg.add_player(new Player(7, 4, 5, 5, "GPasDNom", null));
        psg.add_player(new Player(1, 1, 1, 1, "Epitechien", null));
        psg.add_player(new Player(3, 2, 9, 2, "PlayWithCube", null));
        psg.add_player(new Player(1, 4, 2, 9, "Itectori", null));
        default_team.Add("psg", psg);

        Team fr = new Team("France", compo_fr, def_sound, "fr");
        fr.add_player(new Player(1, 4, 2, 9, "Itectori", null));
        fr.add_player(new Player(3, 2, 9, 2, "PlayWithCube", null));
        fr.add_player(new Player(8, 7, 5, 7, "Epiteen", null));
        fr.add_player(new Player(7, 4, 5, 5, "GPasDNom", null));
        fr.add_player(new Player(4, 6, 7, 1, "Lombrix", null));
        default_team.Add("fr", fr);
        // ------------------
        selected_team = fr;
    }

	public void ResetDefaultPlayer()
	{
		this.default_player = new Dictionary<string, Player>();
		default_player.Add("lombrix", 	new Player(4, 6, 7, 1, "Lombrix", null));
		default_player.Add("itec", 		new Player(1, 4, 2, 9, "Itectori", null));
		default_player.Add("gpdn", 		new Player(7, 4, 5, 5, "GPasDNom", null));
		default_player.Add("pwc", 		new Player(3, 2, 9, 2, "PlayWithCube", null));
		default_player.Add("ept", 		new Player(1, 1, 1, 1, "Epitechien", null));
		default_player.Add("epta", 		new Player(8, 7, 5, 7, "Epiteen", null));
	}
	public void ResetSpecialPlayer()
	{
		this.paid_player = new Dictionary<string, Player>();
		this.secret_player = new Dictionary<string, Player>();
	}

    public void AddOrUpdate_Team(Team t)
    {
        if (default_team.ContainsKey(t.Code))
            default_team[t.Code] = t;
        else
            default_team.Add(t.Code, t);
    }
	public void AddOrUpdate_Player(Player p)
	{
		var players = TypeToDict (p.Type);
		if (players.ContainsKey(p.UID))
			players[p.UID] = p;
		else
			players.Add(p.UID, p);
	}
	public void BuyPlayer(string uid, PlayerType type = PlayerType.Buy)
    {
		var players = TypeToDict (type);
		Player p = players[uid];
		default_player.Add(uid, p);
    }

	private Dictionary<string, Player> TypeToDict(PlayerType type)
	{
		switch (type) {
		case PlayerType.Buy:
			return paid_player;
		case PlayerType.Secret:
			return secret_player;
		default:
			return null;
		}
	}

    public NotificationState NotificationState
    {
        get { return notificationState; }
        set { notificationState = value; }
    }
    public Dictionary<string, Team> Default_Team
    {
        get { return default_team; }
    }
    public Dictionary<string, Player> Default_player
    {
        get { return default_player; }
    }
    public Dictionary<string, Composition> Default_compo
    {
        get { return default_compo; }
    }
    public string Selected_Stadium
    {
        get { return selected_stadium_name; }
        set { Instance.selected_stadium_name = value; }
    }
    public Team Selected_Team
    {
        get { return selected_team; }
        set { Instance.selected_team = value; }
    }
    public Team Random_Team
    {
        get
        {
            var teams = default_team;
            teams.Remove(selected_team.Code);
            int alea = rand.Next(teams.Count);
            int i = 0;
            foreach (var team in teams)
            {
                if (i == alea)
                    return team.Value;
                i++;
            }
            return selected_team;
        }
	}
	public Player Random_Secret_Player
	{
		get 
		{
			int size = secret_player.Count;
			int alea = rand.Next (size * 100);

			int i = 0;
			foreach (var item in secret_player) {
				Player p = item.Value;
				i += p.Proba;
				if (alea < size * i) {
					return p;
				}
			}
			return secret_player.First ().Value; // Théoriquement jamais call
		}
	}
}