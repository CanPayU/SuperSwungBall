﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Player
{
	private string player_name;
	public string Name {
		get { return player_name; }
	}
	private int team_id;

    private Dictionary<string, int> defaultStats = new Dictionary<string, int>(); // Stats initiales (unique au joueur)
    private List<string> buttonsValues = new List<string> { "esquive","esquive","esquive" }; // Valeurs des boutons
    private Dictionary<string, int> finalStats = new Dictionary<string, int> { { "esquive", 0 }, { "tacle", 0 }, { "passe", 0 }, { "course", 0 } }; // Stats après selection des actions dans le menu

	public Player(int tacle, int esquive, int passe, int course, string player_name_, int Team_Id)
    {
        defaultStats.Add("esquive", esquive);
        defaultStats.Add("tacle", tacle);
        defaultStats.Add("passe", passe);
        defaultStats.Add("course", course);
        initialize_finaleStats();
		player_name = player_name_;
		team_id = Team_Id;
    }
    private void initialize_finaleStats() // Initialises les stats finales
    {
        finalStats["tacle"] = 0;
        finalStats["esquive"] = 0;
        finalStats["passe"] = 0;
        finalStats["course"] = defaultStats["course"] + 10;
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
            finalStats[s] += defaultStats[s];
        }
    }

    // Valeurs à changer pour l'equilibrage
    #region Getters
    public float Speed
    {
		get { return (float)defaultStats["course"] / 10; }
	}
	public float Esquive
	{
		get { return (float)defaultStats["esquive"] / 10; }
	}
	public float Passe
	{
		get { return (float)defaultStats["passe"] / 10; }
	}
	public float Tacle
	{
		get { return (float)defaultStats["tacle"] / 10; }
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
		get { return GameObject.Find (player_name); }
	}
    #endregion
}

