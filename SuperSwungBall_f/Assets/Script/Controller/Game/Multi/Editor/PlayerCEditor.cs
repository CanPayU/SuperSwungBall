using UnityEngine;
using UnityEditor;

namespace GameScene.Multi {
	
	[CustomEditor(typeof(PlayerController))]
	public class PlayerCEditor : Editor {

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector ();

			PlayerController myTarget = (PlayerController)target;

			EditorGUILayout.LabelField("IsMine", myTarget.IsMine.ToString());
			EditorGUILayout.LabelField("PointPasse", myTarget.PointPasse.ToString());
			EditorGUILayout.LabelField("PointDeplacement", myTarget.PointDeplacement.ToString());
			EditorGUILayout.Separator ();
			EditorGUILayout.LabelField("ID", myTarget.Player.ID.ToString());
			EditorGUILayout.LabelField("Name", myTarget.Player.Name.ToString());
			EditorGUILayout.LabelField("Team_id", myTarget.Player.Team_id.ToString());
			EditorGUILayout.LabelField("Speed", myTarget.Player.Speed.ToString());
			EditorGUILayout.LabelField("Passe", myTarget.Player.Passe.ToString());
			EditorGUILayout.LabelField("Tacle", myTarget.Player.Tacle.ToString());
			EditorGUILayout.LabelField("ZonePasse", myTarget.Player.ZonePasse.ToString());
			EditorGUILayout.LabelField("ZoneDeplacement", myTarget.Player.ZoneDeplacement.ToString());
		}
	}
}
