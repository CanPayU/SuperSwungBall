using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;


namespace TranslateKit {

	public class Data {

		public JSONObject obj;

		public Data(JSONObject obj){
			this.obj = obj;
			//this.standingTitle = obj.GetString ("standingTitle");
		}
	
	}

	public sealed class TradValues
	{

		public sealed class Standing {
			public static readonly TradValues Title			= new TradValues("standingTitle");
			public static readonly TradValues ClassicConnectionButton = new TradValues("classicConnectionButton");
			public static readonly TradValues tips1		= new TradValues("msgstandingrandom1");
			public static readonly TradValues tips2		= new TradValues("msgstandingrandom2");
			public static readonly TradValues tips3		= new TradValues("msgstandingrandom3");
			public static readonly TradValues tips4		= new TradValues("msgstandingrandom4");
			public static readonly TradValues tips5		= new TradValues("msgstandingrandom5");
			public static readonly TradValues tips6		= new TradValues("msgstandingrandom6");
			public static readonly TradValues tips7		= new TradValues("msgstandingrandom7");
			public static readonly TradValues tips9		= new TradValues("msgstandingrandom9");
			public static readonly TradValues tips8		= new TradValues("msgstandingrandom8");
			public static readonly TradValues tips10	= new TradValues("msgstandingrandom10");
			public static readonly TradValues tips11	= new TradValues("msgstandingrandom11");
			public static readonly TradValues tips12	= new TradValues("msgstandingrandom12");
			public static readonly TradValues tips13	= new TradValues("msgstandingrandom13");
		}

		public sealed class General {
			public static readonly TradValues All		= new TradValues("tout");
			public static readonly TradValues Private	= new TradValues("prive");
			public static readonly TradValues Nothing	= new TradValues("aucun");
		}

		private TradValues(string value)
		{
			Value = value;
		}

		public string Value { get; private set; }
	}
}