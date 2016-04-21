using UnityEngine;
using System;
using System.Collections;

[Serializable]
public struct PlayerStats {

	private float p; // passe
	private float c; // course
	private float e; // esquive
	private float t; // tacle

	public PlayerStats (float passe, float course, float esquive, float tacle) 
	{
		this.p = passe;
		this.c = course;
		this.e = esquive;
		this.t = tacle;
	}

	public override string ToString ()
	{
		return string.Format ("PCET({0:F1}, {1:F1}, {2:F1}, {3:F1})", new object[] {
			this.p,
			this.c,
			this.e,
			this.t
		});
	}

	//
	// Indexer
	//
	public float this [string index] {
		get {
			switch (index) {
			case "passe":
				return this.p;
			case "course":
				return this.c;
			case "esquive":
				return this.e;
			case "tacle":
				return this.t;
			default:
				Debug.LogError ("Invalid String : " + index);
				return 0;
			}
		}
		set {
			switch (index) {
			case "passe":
				this.p = value;
				break;
			case "course":
				this.c = value;
				break;
			case "esquive":
				this.e = value;
				break;
			case "tacle":
				this.t = value;
				break;
			default:
				Debug.LogError ("Invalid String : " + index);
				break;
			}
		}
	}

	/// <summary> Valeur de la passe <summary>
	public float Passe 
	{
		get { return this.p; }
		set { this.p = value; }
	}

	/// <summary> Valeur de la course <summary>
	public float Course 
	{
		get { return this.c; }
		set { this.c = value; }
	}

	/// <summary> Valeur de l'esquive <summary>
	public float Esquive 
	{
		get { return this.e; }
		set { this.e = value; }
	}

	/// <summary> Valeur du tacle <summary>
	public float Tacle 
	{
		get { return this.t; }
		set { this.t = value; }
	}


	//
	// Operators
	//
	public static PlayerStats operator + (PlayerStats a, PlayerStats b) 
	{
		return new PlayerStats (a.p + b.p, a.c + b.c, a.e + b.e, a.t + b.t);
	}
	public static PlayerStats operator - (PlayerStats a, PlayerStats b) 
	{
		return new PlayerStats (a.p - b.p, a.c - b.c, a.e - b.e, a.t - b.t);
	}
	public static PlayerStats operator ++ (PlayerStats a) 
	{
		return new PlayerStats (++a.p, ++a.c, ++a.e, ++a.t);
	}
	public static PlayerStats operator -- (PlayerStats a) 
	{
		return new PlayerStats (--a.p, --a.c, --a.e, --a.t);
	}
}
