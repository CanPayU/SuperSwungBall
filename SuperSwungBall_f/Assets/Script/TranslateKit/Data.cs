using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;


namespace TranslateKit {

	public class Data {

		public string standingTitle;


		public Data(JSONObject obj){
			this.standingTitle = obj.GetString ("standingTitle");
		}

	}

}