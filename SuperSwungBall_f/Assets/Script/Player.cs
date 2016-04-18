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
	public string Name {
		get { return player_name; }
		set { player_name = value; }
	}
	private int team_id;
	private string uid;
	public string UID {
		get { return uid; }
	}
	private int price;
	public int Price {
		get { return price; }
		set { price = value; }
	}

    private Dictionary<string, int> DEFAULTSTATS = new Dictionary<string, int>(); // Stats initiales (unique au joueur)
    private List<string> buttonsValues = new List<string> { "esquive","esquive","esquive" }; // Valeurs des boutons
    private Dictionary<string, int> finalStats = new Dictionary<string, int> { { "esquive", 0 }, { "tacle", 0 }, { "passe", 0 }, { "course", 0 } }; // Stats après selection des actions dans le menu

	public Player(int tacle, int esquive, int passe, int course, string player_name_, string uid, int Team_Id = 0, int price = 0)
    {
        this.DEFAULTSTATS.Add("esquive", esquive);
        this.DEFAULTSTATS.Add("tacle", tacle);
        this.DEFAULTSTATS.Add("passe", passe);
        this.DEFAULTSTATS.Add("course", course);
        initialize_finaleStats();
		this.player_name = player_name_;
		this.team_id = Team_Id;
		this.uid = uid;
		this.price = price;
    }
	public Player (JSONObject json) {
		this.DEFAULTSTATS.Add("esquive", 0); // a gérer
		this.DEFAULTSTATS.Add("tacle", 0);
		this.DEFAULTSTATS.Add("passe", 0);
		this.DEFAULTSTATS.Add("course", 0);
		initialize_finaleStats();
		this.player_name = json.GetString("name");
		this.team_id = 0;
		this.uid = json.GetString("uid");
		this.price = (int)json.GetNumber ("price");
	}

    private void initialize_finaleStats() // Initialises les stats finales
    {
        finalStats["tacle"] = 0;
        finalStats["esquive"] = 0;
		finalStats["passe"] = 0;
        finalStats["course"] = DEFAULTSTATS["course"] + 10;
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
		//Debug.Log ("Compute State : Passe:" + finalStats ["passe"] + " for " + Name);
    }

    // Valeurs à changer pour l'equilibrage
    #region Getters
    public float Speed
    {
		get { return (float)finalStats["course"] / 10; }
	}
	public float Esquive
	{
		get { return (float)finalStats["esquive"] / 10; }
	}
	public float Passe
	{
		get { return (float)finalStats["passe"] / 10; }
	}
	public float Tacle
	{
		get { return (float)finalStats["tacle"] / 10; }
	}
    public float SpeedBase
    {
        get { return (float)DEFAULTSTATS["course"] / 10; }
    }
    public float EsquiveBase
    {
        get { return (float)DEFAULTSTATS["esquive"] / 10; }
    }
    public float PasseBase
    {
        get { return (float)DEFAULTSTATS["passe"] / 10; }
    }
    public float TacleBase
    {
        get { return (float)DEFAULTSTATS["tacle"] / 10; }
    }
    public float ZoneDeplacement
    {
        get { return (float)finalStats["course"] / 10; }
    }
    public float ZonePasse
    {
        get { return (float)finalStats["passe"] / 10; }
	}
	public GameObject Gm
	{
		get { return GameObject.Find (player_name+"-"+team_id); }
	}
	public int Team_id {
		get { return team_id; }
		set { team_id = value; }
	}
	public List<string> Button_Values {
		get { return buttonsValues; }
		set { buttonsValues = value;
			computeStats ();}
	}
    #endregion
}

