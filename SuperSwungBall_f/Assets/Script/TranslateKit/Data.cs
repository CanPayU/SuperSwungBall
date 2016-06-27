using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;


namespace TranslateKit
{

    public class Data
    {

        public JSONObject obj;

        public Data(JSONObject obj)
        {
            this.obj = obj;
            //this.standingTitle = obj.GetString ("standingTitle");
        }

    }

    public sealed class TradValues
    {

        public sealed class Standing
        {
            public static readonly TradValues Title = new TradValues("standingTitle");
            public static readonly TradValues ClassicConnectionButton = new TradValues("classicConnectionButton");
            public static readonly TradValues tips1 = new TradValues("msgstandingrandom1");
            public static readonly TradValues tips2 = new TradValues("msgstandingrandom2");
            public static readonly TradValues tips3 = new TradValues("msgstandingrandom3");
            public static readonly TradValues tips4 = new TradValues("msgstandingrandom4");
            public static readonly TradValues tips5 = new TradValues("msgstandingrandom5");
            public static readonly TradValues tips6 = new TradValues("msgstandingrandom6");
            public static readonly TradValues tips7 = new TradValues("msgstandingrandom7");
            public static readonly TradValues tips9 = new TradValues("msgstandingrandom9");
            public static readonly TradValues tips8 = new TradValues("msgstandingrandom8");
            public static readonly TradValues tips10 = new TradValues("msgstandingrandom10");
            public static readonly TradValues tips11 = new TradValues("msgstandingrandom11");
            public static readonly TradValues tips12 = new TradValues("msgstandingrandom12");
            public static readonly TradValues tips13 = new TradValues("msgstandingrandom13");
        }

        public sealed class General
        {
            public static readonly TradValues All = new TradValues("tout");
            public static readonly TradValues Private = new TradValues("prive");
            public static readonly TradValues Nothing = new TradValues("aucun");
            public static readonly TradValues Buy = new TradValues("acheter");
            public static readonly TradValues Play = new TradValues("jouer");
            public static readonly TradValues Notification = new TradValues("notification");
            public static readonly TradValues Home = new TradValues("home");
            public static readonly TradValues Settings = new TradValues("reglages");
            public static readonly TradValues Account = new TradValues("compte");
            public static readonly TradValues Deconnection = new TradValues("deconnexion");
            public static readonly TradValues Music = new TradValues("musique");
            public static readonly TradValues Pass = new TradValues("passe");
            public static readonly TradValues Effect = new TradValues("effect");
            public static readonly TradValues Song = new TradValues("son");
            public static readonly TradValues Course = new TradValues("course");
            public static readonly TradValues Esquive = new TradValues("esquive");
            public static readonly TradValues Tacle = new TradValues("tacle");
            public static readonly TradValues Team = new TradValues("equipe");
            public static readonly TradValues Tuto = new TradValues("tuto");
            public static readonly TradValues Shop = new TradValues("boutique");
            public static readonly TradValues Leave = new TradValues("quitter");
            public static readonly TradValues Friends = new TradValues("amis");
            public static readonly TradValues Load = new TradValues("charge");
            public static readonly TradValues Points = new TradValues("points");
            public static readonly TradValues Pseudo = new TradValues("pseudo");
            public static readonly TradValues Clean = new TradValues("vider");
            public static readonly TradValues Choose = new TradValues("choisir");
            public static readonly TradValues More = new TradValues("plus");
            public static readonly TradValues Cancel = new TradValues("cancel");
            public static readonly TradValues Sync = new TradValues("sync");
            public static readonly TradValues Langue = new TradValues("langue");
            public static readonly TradValues Save = new TradValues("enregistrer");
        }

        public sealed class Menu
        {
            public static readonly TradValues TouchPass = new TradValues("touchepasse");
        }

        public sealed class Settings
        {
            public static readonly TradValues autoriseSave = new TradValues("autoriseSave");
            public static readonly TradValues autoriseSaveDesc = new TradValues("autoriseSaveDesc");
            public static readonly TradValues cleanSave = new TradValues("cleanSave");
            public static readonly TradValues cleanSaveDesc = new TradValues("cleanSaveDesc");
        }

        public sealed class CreateTeam
        {
            public static readonly TradValues Title = new TradValues("createTTitle");
            public static readonly TradValues SelectPlayer = new TradValues("selectionjoueurtext");
            public static readonly TradValues ChooseCompo = new TradValues("choirircompotext");
            public static readonly TradValues CreateT = new TradValues("creeruneequipe");
            public static readonly TradValues ChhoseName = new TradValues("choissierlenom");
            public static readonly TradValues MoreInfo = new TradValues("moreInfo");
            public static readonly TradValues MoreInfoDesc = new TradValues("moreInfoDesc");
            //			public static readonly TradValues Title	= new TradValues("createTTitle");
            //			public static readonly TradValues Title	= new TradValues("createTTitle");
            //			public static readonly TradValues Title	= new TradValues("createTTitle");
            //			public static readonly TradValues Title	= new TradValues("createTTitle");
            //			public static readonly TradValues Title	= new TradValues("createTTitle");
            //			public static readonly TradValues Title	= new TradValues("createTTitle");
            //			public static readonly TradValues Title	= new TradValues("createTTitle");
        }




        //		"phraseinfo1" : "Créez, modifiez votre équipe!",
        //		"phraseinfo2" : "Ajoutez les joueurs que vous venez de gagner ou",
        //		"phraseinfo3" : "Acheter!",
        //		"phraseinfo4" : "Les Ducats permettent de créer une équipe",
        //		"phraseinfo5" : "équilibrée. Chaques joueurs coute maximum 10",
        //		"phraseinfo6" : "Ducats. Vous pouvez en comptabiliser jusqu'à 25",
        //		"phraseinfo7" : "dans une équipe!",

        private TradValues(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}