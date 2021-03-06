﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

using TranslateKit;

[System.Serializable]
public class Settings
{

    private static Settings _instance = new Settings();
    public static Settings Instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    public const string VERSION = "1.34"; // Version actuelle
    public string version; // Version de l'instance (sauvegarder sur l'ordi)

    private Dictionary<string, Team> default_team;
    private Dictionary<string, Player> default_player;
    private Dictionary<string, Composition> default_compo;

    private Team selected_team;
    private string selected_stadium_name;
    private System.Random rand = new System.Random();

    private Dictionary<string, Player> paid_player;
    private Dictionary<string, Player> secret_player; // in Chest
    private Dictionary<string, Player> challenge_player; // in Challenge

    // -- Keyboard
    private Dictionary<KeyboardAction, KeyCode> keyboard;
    // --

    private NotificationState notificationState;
    private SoundState soundState;

    private bool allowReplayBackup;

    private AvailableLanguage selectedLanguage;

    public Settings()
    {
        this.version = VERSION;
        this.notificationState = NotificationState.All;
        this.soundState = SoundState.All;
        this.keyboard = new Dictionary<KeyboardAction, KeyCode>();
        this.paid_player = new Dictionary<string, Player>();
        this.secret_player = new Dictionary<string, Player>();
        this.challenge_player = new Dictionary<string, Player>();
        this.allowReplayBackup = true;
        this.selectedLanguage = AvailableLanguage.FR;

        // -- Setup Keyboard
        this.keyboard.Add(KeyboardAction.Passe, KeyCode.A);
        // --

        ResetDefaultPlayer();
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
        string[] def_sound = new string[] { "Musics/Team/PSG/Allez", "Musics/Team/PSG/Clap" };
        Team psg = new Team("PSG", compo_psg, def_sound, "psg");

        psg.add_player(new Player(4, 6, 10, 4, "Lombrix", "D_00_lombrix"));
        psg.add_player(new Player(1, 4, 6, 10, "Itectori", "D_01_itectori"));
        psg.add_player(new Player(3, 2, 10, 7, "PlayWithCube", "D_02_pwc"));
        psg.add_player(new Player(7, 4, 4, 8, "GPasDNom", "D_03_gpasdnom"));

        //psg.add_player(new Player(4, 6, 10, 4, "Lombrix"));
        //psg.add_player(new Player(1, 4, 6, 10, "Itectori"));
        //psg.add_player(new Player(3, 2, 10, 7, "PlayWithCube"));
        //psg.add_player(new Player(7, 4, 4, 8, "GPasDNom"));

        psg.add_player(new Player(1, 1, 6, 8, "Epitechien"));
        default_team.Add("psg", psg);

        def_sound = new string[] { };
        Team fr = new Team("France", compo_fr, def_sound, "fr");

        fr.add_player(new Player(4, 6, 10, 4, "Lombrix", "D_00_lombrix"));
        fr.add_player(new Player(1, 4, 6, 10, "Itectori", "D_01_itectori"));
        fr.add_player(new Player(3, 2, 10, 7, "PlayWithCube", "D_02_pwc"));
        fr.add_player(new Player(7, 4, 4, 8, "GPasDNom", "D_03_gpasdnom"));

        //fr.add_player(new Player(4, 6, 10, 4, "Lombrix"));
        //fr.add_player(new Player(1, 4, 6, 10, "Itectori"));
        //fr.add_player(new Player(3, 2, 10, 7, "PlayWithCube"));
        //fr.add_player(new Player(7, 4, 4, 8, "GPasDNom"));

        fr.add_player(new Player(8, 7, 5, 7, "Epiteen"));
        default_team.Add("fr", fr);
        // ------------------
        selected_team = fr;
    }

    public void ResetDefaultPlayer()
    {
        this.default_player = new Dictionary<string, Player>();
        default_player.Add("lombrix", new Player(4, 6, 10, 4, "Lombrix", "D_00_lombrix"));
        default_player.Add("itec", new Player(1, 4, 6, 10, "Itectori", "D_01_itectori"));
        default_player.Add("pwc", new Player(3, 2, 10, 7, "PlayWithCube", "D_02_pwc"));
        default_player.Add("gpdn", new Player(7, 4, 4, 8, "GPasDNom", "D_03_gpasdnom"));
        default_player.Add("ept", new Player(1, 1, 6, 8, "Epitechien"));
        default_player.Add("epta", new Player(8, 7, 5, 7, "Epiteen"));
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
        var players = TypeToDict(p.Type);
        if (players.ContainsKey(p.UID))
            players[p.UID] = p;
        else
            players.Add(p.UID, p);
    }
    public void UpdateKeyboard(KeyboardAction action, KeyCode code)
    {
        this.keyboard[action] = code;
    }
    public void BuyPlayer(string uid, PlayerType type = PlayerType.Buy)
    {
        var players = TypeToDict(type);
        Player p = players[uid];
        default_player.Add(uid, p);
    }

    private Dictionary<string, Player> TypeToDict(PlayerType type)
    {
        switch (type)
        {
            case PlayerType.Buy:
                return paid_player;
            case PlayerType.Secret:
                return secret_player;
            case PlayerType.Challenge:
                return challenge_player;
            default:
                return null;
        }
    }


    public AvailableLanguage SelectedLanguage
    {
        get { return this.selectedLanguage; }
        set { this.selectedLanguage = value; }
    }
    public bool AllowReplayBackup
    {
        get { return this.allowReplayBackup; }
        set { this.allowReplayBackup = value; }
    }
    public NotificationState NotificationState
    {
        get { return notificationState; }
        set { notificationState = value; }
    }
    public SoundState SoundState
    {
        get { return soundState; }
        set { soundState = value; }
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
    public Dictionary<KeyboardAction, KeyCode> Keyboard
    {
        get { return keyboard; }
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
            var teams = new Dictionary<string, Team>(default_team);
            teams.Remove(selected_team.Code);
            int alea = rand.Next(teams.Count);
            int i = 0;
            foreach (var team in teams)
            {
                if (i == alea)
                    return team.Value;
                i++;
            }
            return selected_team; // Théoriquement jamais call
        }
    }
    public Player Random_Player
    {
        get
        {
            var players = new Dictionary<string, Player>(default_player);
            int alea = rand.Next(players.Count);
            int i = 0;
            foreach (var player in players)
            {
                if (i == alea)
                    return player.Value;
                i++;
            }
            return players.First().Value; // Théoriquement jamais call
        }
    }
    public Player Random_Secret_Player
    {
        get
        {
            int size = secret_player.Count;
            int alea = rand.Next(size * 100);

            int i = 0;
            foreach (var item in secret_player)
            {
                Player p = item.Value;
                i += p.Proba;
                if (alea < size * i)
                {
                    return p;
                }
            }
            return secret_player.First().Value; // Théoriquement jamais call
        }
    }
}