using UnityEngine;
using UnityEditor;
using NUnit.Framework;

[CustomEditor(typeof(ClientManager))]
public class ClientEditor : Editor {

	public override void OnInspectorGUI()
	{
		ClientManager myTarget = (ClientManager)target;
		Client client = myTarget.Client;

		if (Application.isPlaying) {
			EditorGUILayout.HelpBox ("Vérifier que les donné son à jour", MessageType.Info);
			EditorGUILayout.LabelField ("Host", client.Host);
			EditorGUILayout.LabelField ("Authenticated", client.IsAuthticate.ToString ());
		} else {
			EditorGUILayout.HelpBox ("Host et Authentificated disponible seulement en PlayMode", MessageType.Info);
		}
	}
}