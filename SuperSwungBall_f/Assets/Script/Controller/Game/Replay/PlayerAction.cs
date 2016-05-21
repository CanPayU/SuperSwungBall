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
		private Vector3Serializer pointPasse; // passe vers ...
		private List<string> btnValues; // atouts


//		private bool FaitUnePasse;
//		private Vector3 PosWhenHeDoPasse;

		public PlayerAction(int id, Vector3 pointDep, Vector3 pointPasse, List<string> btnValues) {
			this.id = id;
			this.pointDep = pointDep;
			this.pointPasse = pointPasse;
			this.btnValues = btnValues;
		}

		/// <summary>
		/// retourne le type sous la forme object.
		/// Utile pour le synchro Photon.
		/// </summary>
		public void GetObject(){}

		public PlayerAction UpdateWith(PlayerAction action) {
			this.pointDep = action.pointDep;
			this.pointPasse = action.pointPasse;
			this.btnValues = action.btnValues;
			return this;
		}

		public override string ToString ()
		{
			return string.Format ("[PlayerAction: Deplacement={0}, Passe={1}, ButtonValuesCount={2}]", Deplacement, Passe, ButtonValues.Count);
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
		public Vector3 Passe {
			get { return this.pointPasse; }
			set { this.pointPasse = value; }
		}
		public List<string> ButtonValues {
			get { return this.btnValues; }
			set { this.btnValues = value; }
		}
	}

	[System.Serializable]
	public class Vector3Serializer
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