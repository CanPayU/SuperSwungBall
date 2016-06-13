using UnityEngine;
using UnityEditor;
using System.Collections;

using TranslateKit;

namespace Extension.UI
{
	[CustomEditor(typeof(TextDerived))]
	public class TextDerivedEditor : Editor
	{

		TextDerived obj;
		string[] sceneValues;
		int sceneIndex = 0;

		string[] values;
		int valuesIndex = 0;

		public TextDerivedEditor(){
			var props = typeof(TradValues).GetNestedTypes ();
			var len = props.Length;
			this.sceneValues = new string[len];

			for (int i = 0; i < len; i++) {
				sceneValues [i] = props [i].Name;
			}
			loadValues ();
		}

		public override void OnInspectorGUI()
		{
			this.obj = (TextDerived)target;
			
			GUILayout.Space(15);
			EditorGUILayout.HelpBox ("Text traduit du label. Si tu changes de scene, update les valeurs", MessageType.Info);
			sceneIndex = EditorGUILayout.Popup(sceneIndex, sceneValues);
			valuesIndex = EditorGUILayout.Popup(valuesIndex, values);

			GUILayout.Space(3);

			if(GUILayout.Button("Update Values"))
				loadValues ();

			if(GUILayout.Button("Search Value"))
				obj.trad = getValue();
			
			EditorGUILayout.TextField ("Value", obj.trad);

			EditorGUILayout.Separator();

			DrawDefaultInspector();
		}

		private void loadValues(){
			this.valuesIndex = 0;
			var props = typeof(TradValues).GetNestedType(sceneValues [sceneIndex]).GetFields ();
			var len = props.Length;
			this.values = new string[len];

			for (int i = 0; i < len; i++) {
				values [i] = props [i].Name;
			}
		}

		private string getValue(){
			var trad = getTrad();
			if (trad != null)
				return trad.Value;
			else
				return "Error Not Found";
		}
		private TradValues getTrad(){
			var val = typeof(TradValues).GetNestedType (sceneValues [sceneIndex]).GetField (values [valuesIndex]);
			if (val != null)
				return (TradValues)val.GetValue (null);
			else
				return null;
		}
	}
}