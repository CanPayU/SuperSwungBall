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
            tips = File.ReadAllLines("Assets/Resources/tips.txt");
            rand = new System.Random();
            r_ = rand.Next(tips.Length);
            tip_text.text = tips[r_];
        }
    }
}

