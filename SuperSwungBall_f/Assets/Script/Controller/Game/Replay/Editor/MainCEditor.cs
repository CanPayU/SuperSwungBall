using UnityEngine;
using UnityEditor;
using NUnit.Framework;


namespace GameScene.Replay
{

	[CustomEditor(typeof(MainController))]
	public class MainCEditor : Editor
	{

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			MainController myTarget = (MainController)target;

			EditorGUILayout.LabelField ("Replay Main");
		}
	}
}
