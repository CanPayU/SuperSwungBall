using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

namespace TranslateKit
{

    public static class Language
    {

        private static Data data;
        public static AvailableLanguage language;

        public static void LoadLanguage(AvailableLanguage uid)
        {
            string jsonString = ((TextAsset)Resources.Load("Translate\\" + uid.Value, typeof(TextAsset))).text;
            JSONObject json = JSONObject.Parse(jsonString);

            if (json.GetString("language") == uid.Value)
            {
                JSONObject languageObj = json.GetObject("data");
                data = new Data(languageObj);
                language = uid;
            }
        }

        public static string GetValue(TradValues value)
        {
            if (data != null)
                return data.obj.GetString(value.Value);
            return "";
        }

        public static string GetValue(string value)
        {
            if (data != null)
                return data.obj.GetString(value);
            return "";
        }
    }

    [System.Serializable]
    public sealed class AvailableLanguage
    {
        public static readonly AvailableLanguage FR = new AvailableLanguage("FR");
        public static readonly AvailableLanguage EN = new AvailableLanguage("EN");

        private AvailableLanguage(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }

    //	public enum AvailableLanguage {
    //		FR,		// Français
    //		EN		// Anglais
    //	}

}