﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using TranslateKit;

namespace Extension.UI
{
    public class TextDerived : Text
    {

        public string trad;

        protected override void Start()
        {
            base.OnEnable();
            if (this.trad != null && this.trad != "")
                this.text = Language.GetValue(this.trad);
        }
    }
}