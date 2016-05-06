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
    public string Name
    {
        get { return player_name; }
        set { player_name = value; }
    }
    private int team_id;
    private string uid;
    public string UID
    {
        get { return uid; }
    }
    private int price;
    public int Price // En Phi
    {
        get { return price; }
        set { price = value; }
	}
	private PlayerType type;
	public PlayerType Type
	{
		get { return type; }
		set { type = value; }
	}
	private int proba; // Pourcentage
	public int Proba
	{
		get { return proba; }
		set { proba = value; }
	}
	private float ducat; // Cout d'un player dans une team

	private PlayerStats DEFAULTSTATS = new PlayerStats(0, 0, 0, 0); // Stats initiales (unique au joueur)
    private List<string> buttonsValues = new List<string> { "esquive", "esquive", "esquive" }; // Valeurs des boutons
	private PlayerStats finalStats = new PlayerStats(0, 0, 0, 0); // Stats après selection des actions dans le menu

	public Player(int tacle, int esquive, int passe, int course, string player_name_, string uid = "IdPlayer", int Team_Id = 0, int price = 0, int ducat = -1)
    {
		this.DEFAULTSTATS.Esquive = esquive * 10;
		this.DEFAULTSTATS.Tacle = tacle * 10;
		this.DEFAULTSTATS.Passe = passe * 10;
		this.DEFAULTSTATS.Course = course * 10;
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

		var stats = json.GetObject ("stats");
		this.DEFAULTSTATS.Esquive = (int)stats.GetNumber("esquive") * 10;
		this.DEFAULTSTATS.Tacle = (int)stats.GetNumber("tacle") * 10;
		this.DEFAULTSTATS.Passe = (int)stats.GetNumber("passe") * 10;
		this.DEFAULTSTATS.Course = (int)stats.GetNumber("course") * 10;
		initialize_finaleStats();
    }

    private void initialize_finaleStats() // Initialises les stats finales
    {
		finalStats.Tacle = 0;
		finalStats.Esquive = 0;
		finalStats.Passe = 0;
		finalStats.Course = DEFAULTSTATS.Course + 10;
    }

	private void calculateDucat(){
		float sum = this.DEFAULTSTATS.Esquive + this.DEFAULTSTATS.Passe + this.DEFAULTSTATS.Course + this.DEFAULTSTATS.Tacle;
		// Combo
		float ce = this.DEFAULTSTATS.Course + this.DEFAULTSTATS.Esquive; 	// bien
		float pe = this.DEFAULTSTATS.Passe + this.DEFAULTSTATS.Esquive; 	// bien
		float ct = this.DEFAULTSTATS.Course + this.DEFAULTSTATS.Tacle; 		// bien
		float cp = this.DEFAULTSTATS.Course + this.DEFAULTSTATS.Passe; 		// mal
		float te = this.DEFAULTSTATS.Tacle + this.DEFAULTSTATS.Esquive; 	// mal
		float pt = this.DEFAULTSTATS.Passe + this.DEFAULTSTATS.Tacle; 		// mal
		float finalCombo = ce + pe + ct - cp - te - pt;

		this.ducat = ((sum + finalCombo) / 4)/100;
	}

    public void reset() // reinitialises valeurs des boutons et les stats finales
    {
        buttonsValues[0] = "esquive";
        buttonsValues[1] = "esquive";
        buttonsValues[2] = "esquive";
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
			finalStats[s] += DEFAULTSTATS[s];
        }
    }

    // Valeurs à changer pour l'equilibrage
    #region Getters
    public float Speed
    {
		get { return (float)finalStats.Course / 100; }
    }
    public float Esquive
    {
		get { return (float)finalStats.Esquive / 100; }
    }
    public float Passe
    {
		get { return (float)finalStats.Passe / 100; }
    }
    public float Tacle
    {
		get { return (float)finalStats.Tacle / 100; }
    }
    public float SpeedBase
    {
		get { return (float)DEFAULTSTATS.Course / 100; }
    }
    public float EsquiveBase
    {
		get { return (float)DEFAULTSTATS.Esquive / 100; }
    }
    public float PasseBase
    {
		get { return (float)DEFAULTSTATS.Passe / 100; }
    }
    public float TacleBase
    {
		get { return (float)DEFAULTSTATS.Tacle / 100; }
    }
    public float ZoneDeplacement
    {
		get { return (float)finalStats.Course / 100; }
    }
    public float ZonePasse
    {
		get { return (float)finalStats.Passe / 100; }
    }
	public float Ducat
	{
		get { 
			if (ducat > -1)
				return ducat;
			else {
				calculateDucat ();
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

