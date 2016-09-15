using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Boomlagoon.JSON;

[Serializable]
public class Player
{
    private string player_name;
    private int team_id;
    private string uid;
    private float ducat; // Cout d'un player dans une team

    // --- Pour la gestion
    private int price;
    private PlayerType type;
    private int proba; // Pourcentage

    private bool ballHolder = false; // porte la balle

    private PlayerStats DEFAULTSTATS = new PlayerStats(0, 0, 0, 0); // Stats initiales (unique au joueur)
    private List<string> buttonsValues = new List<string> { "esquive", "esquive", "esquive" }; // Valeurs des boutons
    private PlayerStats finalStats = new PlayerStats(0, 0, 0, 0); // Stats après selection des actions dans le menu
    private PlayerStats bonus = new PlayerStats(0, 0, 0, 0); // bonus / malus

    public Player(int tacle, int esquive, int passe, int course, string player_name_, string uid = "IdPlayer", int Team_Id = 0, int price = 0, int ducat = -1)
    {
        this.DEFAULTSTATS.Esquive = esquive;
        this.DEFAULTSTATS.Tacle = tacle;
        this.DEFAULTSTATS.Passe = passe;
        this.DEFAULTSTATS.Course = course;
        initialize_finaleStats();
        this.player_name = player_name_;
        this.team_id = Team_Id;
        this.uid = uid;
        this.price = price;
        this.ducat = ducat;
    }
    public Player(JSONObject json)
    {
        this.ducat = -1;
        this.player_name = json.GetString("name");
        this.team_id = 0;
        this.uid = json.GetString("uid");
        this.type = (PlayerType)Enum.Parse(typeof(PlayerType), json.GetString("type"));
        this.price = (int)json.GetNumber("price");
        this.proba = (int)json.GetNumber("proba");

        var stats = json.GetObject("stats");
        this.DEFAULTSTATS.Esquive = (int)stats.GetNumber("esquive");
        this.DEFAULTSTATS.Tacle = (int)stats.GetNumber("tacle");
        this.DEFAULTSTATS.Passe = (int)stats.GetNumber("passe");
        this.DEFAULTSTATS.Course = (int)stats.GetNumber("course");
        initialize_finaleStats();
    }

    private void initialize_finaleStats() // Initialises les stats finales
    {
        finalStats.Tacle = 0;
        finalStats.Esquive = 0;
        finalStats.Passe = 0;
        finalStats.Course = DEFAULTSTATS.Course;
    }

    private void calculateDucat()
    {
        float sum = this.DEFAULTSTATS.Esquive + this.DEFAULTSTATS.Passe + this.DEFAULTSTATS.Course + this.DEFAULTSTATS.Tacle;
        // Combo
        float ce = this.DEFAULTSTATS.Course + this.DEFAULTSTATS.Esquive;    // bien
        float pe = this.DEFAULTSTATS.Passe + this.DEFAULTSTATS.Esquive;     // bien
        float ct = this.DEFAULTSTATS.Course + this.DEFAULTSTATS.Tacle;      // bien
        float cp = this.DEFAULTSTATS.Course + this.DEFAULTSTATS.Passe;      // mal
        float te = this.DEFAULTSTATS.Tacle + this.DEFAULTSTATS.Esquive;     // mal
        float pt = this.DEFAULTSTATS.Passe + this.DEFAULTSTATS.Tacle;       // mal
        float finalCombo = ce + pe + ct - cp - te - pt;

        this.ducat = ((sum + finalCombo) / 4) / 10;
    }

    public void reset() // reinitialises valeurs des boutons et les stats finales
    {
        buttonsValues[0] = "esquive";
        buttonsValues[1] = "esquive";
        buttonsValues[2] = "esquive";
        bonus = new PlayerStats(0, 0, 0, 0);
        computeStats();
    }

    public void updateValues(string value) // Change la valeur des boutons
    {
        buttonsValues.Add(value);
        buttonsValues.RemoveAt(0);
        computeStats();
    }
    public void computeStats() // Calcule les Stats finales
    {
        initialize_finaleStats();
        foreach (string s in buttonsValues)
        {
            if (finalStats[s] != 0)
            {
                finalStats[s] += 2;
            }
            else
            {
                finalStats[s] += DEFAULTSTATS[s] + bonus[s];
            }
        }
    }

    // Valeurs à changer pour l'equilibrage
    #region Getters
    public bool BallHolder
    {
        get { return ballHolder; }
        set { ballHolder = value; }
    }
    public string Name
    {
        get { return player_name; }
        set { player_name = value; }
    }
    public string UID
    {
        get { return uid; }
    }
    public int Price // En Phi
    {
        get { return price; }
        set { price = value; }
    }
    public PlayerType Type
    {
        get { return type; }
        set { type = value; }
    }
    public int Proba
    {
        get { return proba; }
        set { proba = value; }
    }
    public float Speed
    {
        get { return (float)finalStats.Course / 10; }
    }
    public float Esquive
    {
        get { return (float)finalStats.Esquive / 10; }
    }
    public float Passe
    {
        get { return (float)finalStats.Passe / 10; }
    }
    public float Tacle
    {
        get { return (float)finalStats.Tacle / 10; }
    }
    public float SpeedBase
    {
        get { return (float)DEFAULTSTATS.Course / 10; }
    }
    public float EsquiveBase
    {
        get { return (float)DEFAULTSTATS.Esquive / 10; }
    }
    public float PasseBase
    {
        get { return (float)DEFAULTSTATS.Passe / 10; }
    }
    public float TacleBase
    {
        get { return (float)DEFAULTSTATS.Tacle / 10; }
    }
    public float ZoneDeplacement
    {
        get { return (float)finalStats.Course / 10 + 0.5f; }
    }
    public float ZonePasse
    {
        get { return Passe == 0 ? 0 : (float)finalStats.Passe / 10 + 0.3f; }
    }
    public float Ducat
    {
        get
        {
            if (ducat > -1)
                return ducat;
            else
            {
                calculateDucat();
                return ducat;
            }
        }
        set { ducat = value; }
    }
    public GameObject Gm
    {
        get { return GameObject.Find(player_name + "-" + team_id); }
    }
    public int Team_id
    {
        get { return team_id; }
        set { team_id = value; }
    }
    public List<string> Button_Values
    {
        get { return buttonsValues; }
        set
        {
            buttonsValues = value;
            computeStats();
        }
    }
    #endregion
}

