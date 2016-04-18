using UnityEngine;
using System.Collections;

namespace Gestion {
	
	public class PlayerController : MonoBehaviour {

		private Player player;
		// NULL - 0 Name - 0
		// Debug.Log(p.Gm + " - " + p.ID + " - " + p.Name + " - " + p.Team_id);

		// Use this for initialization
		void Start () {
			string playerid = "IdPlayer";
			GameObject player_gm = Resources.Load("Prefabs/Resources/"+ playerid) as GameObject;
			Transform player = Instantiate (player_gm).transform;
			player.SetParent (transform, false);

			RectTransform rect = player.GetComponent<RectTransform> ();
			rect.localScale = new Vector3 (200f, 200f, 200f);
			rect.anchoredPosition3D = new Vector3 (0, -35f, -20f);
			rect.rotation = new Quaternion (0, -180f, 0, 0);
		}

		public Player Player {
			get { return this.player; }
			set { this.player = value; }
		}
	}
}