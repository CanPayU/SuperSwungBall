using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

namespace TranslateKit {
	
	public static class Language {

		public static Data data;

		public static void LoadLanguage(string uid){

			string jsonString = ((TextAsset)Resources.Load("TranslateData", typeof(TextAsset))).text;
			JSONObject json = JSONObject.Parse (jsonString);

			if (json.ContainsKey (uid)) {
				JSONObject languageObj = json.GetObject (uid);
				data = new Data (languageObj);
			}
		}
	}


	public enum AvailableLanguage {
		FR,		// Français
		EN		// Anglais
	}

}