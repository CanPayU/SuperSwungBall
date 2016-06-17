using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

using TranslateKit;

namespace Standing
{
    public class TipsController : MonoBehaviour
    {
		private System.Random rand = new System.Random();
//        private string[] tips;
//        private int r_;
        public Text tip_text;

        // Use this for initialization
        void Start()
        {
//            TextAsset ta = Resources.Load("tips", typeof(TextAsset)) as TextAsset;
//            string[] tips = ta.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

//            r_ = rand.Next(tips.Length);
//			tip_text.text = tips[r_];
			var name = "tips"+ (rand.Next(12) + 1);
			var value = (TradValues)(typeof(TradValues.Standing).GetField (name).GetValue(null));
			tip_text.text = Language.GetValue(value);
        }
    }
}

