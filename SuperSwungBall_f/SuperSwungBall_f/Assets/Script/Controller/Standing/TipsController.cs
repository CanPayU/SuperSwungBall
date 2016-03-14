using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

namespace Standing
{
    public class TipsController : MonoBehaviour
    {

        private System.Random rand;
        private string[] tips;
        private int r_;
        public Text tip_text;

        // Use this for initialization
        void Start()
        {
			TextAsset ta = Resources.Load ("tips", typeof(TextAsset)) as TextAsset;
			string[] tips = ta.text.Split (new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            
			rand = new System.Random();
            r_ = rand.Next(tips.Length);
            tip_text.text = tips[r_];
        }
    }
}

