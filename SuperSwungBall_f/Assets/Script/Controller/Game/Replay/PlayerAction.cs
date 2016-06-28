using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameScene.Replay
{
	/// <summary>
	/// Type contenant les actions réalisées le player durant le round
	/// </summary>
	[System.Serializable]
	public struct PlayerAction
	{
		private int id;

		private Vector3Serializer pointDep; // déplacement vers ...
		private List<string> btnValues; // atouts

		private bool makePasse; // Fait une passe ?
		private Vector3Serializer posPasse; // Position de déclenchement de la passe
		private Vector3Serializer pointPasse; // Direction de la passe

		/// <summary>
		/// Constructeurs d'action sans passe
		/// </summary>
		public PlayerAction(int id, Vector3 pointDep, List<string> btnValues) {
			this.id = id;
			this.pointDep = pointDep;
			this.btnValues = btnValues;
			this.makePasse = false;
			this.posPasse = Vector3.zero;
			this.pointPasse = Vector3.zero;
		}

		/// <summary>
		/// Constructeur d'action avec passe
		/// </summary>
		/// <param name="pointPasse">Direction de la passe</param>
		/// <param name="posPasse">Position de déclenchement</param>
		public PlayerAction(int id, Vector3 pointDep, Vector3 pointPasse, List<string> btnValues, Vector3 posPasse)
		{
			this.id = id;
			this.pointDep = pointDep;
			this.pointPasse = pointPasse;
			this.btnValues = btnValues;
			this.makePasse = true;
			this.posPasse = posPasse;
		}

		/// <summary>
		/// retourne le type sous la forme object.
		/// Utile pour le synchro Photon.
		/// </summary>
		public void GetObject(){}

		public PlayerAction UpdateWith(PlayerAction action) {
			this.makePasse = action.makePasse;
			this.posPasse = action.posPasse;
			this.pointDep = action.pointDep;
			this.pointPasse = action.pointPasse;
			this.copyBtnValues(action.btnValues);
			return this;
		}

		private void copyBtnValues(List<string> values) {
			this.btnValues = new List<string>();
			foreach (var item in values)
			{
				this.btnValues.Add(item);
			}
		}

		public override string ToString ()
		{
			string str = "";
			ButtonValues.ForEach((val) =>
			{
				str += val + "-";
			});

			return string.Format ("[PlayerAction: MakePasse={0}, Passe={1}, ButtonValuesCount={2}]", makePasse, Passe, str);
		}

		///
		/// Getters / Setters
		///

		public int Id {
			get { return this.id; }
		}
		public Vector3 Deplacement {
			get { return this.pointDep; }
			set { this.pointDep = value; }
		}
		public List<string> ButtonValues
		{
			get { return this.btnValues; }
			set { this.btnValues = value; }
		}
		public Vector3 Passe {
			get { return this.pointPasse; }
			set { this.pointPasse = value; }
		}
		public Vector3 PosPasse {
			get { return this.posPasse; }
			set { this.posPasse = value; }
		}
		public bool MakePasse
		{
			get { return this.makePasse; }
			set { this.makePasse = value; }
		}
	}

	[System.Serializable]
	public struct Vector3Serializer
	{
		public float x;
		public float y;
		public float z;

		public Vector3Serializer(float x, float y, float z){
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// conversion from Vector3Serializer to Vector3
		public static implicit operator Vector3(Vector3Serializer serializer)
		{
			return new Vector3(serializer.x, serializer.y, serializer.z);
		}
		//  conversion from Vector3 to Vector3Serializer
		public static implicit operator Vector3Serializer(Vector3 v)
		{
			return new Vector3Serializer(v.x, v.y, v.z);
		}

	}
}