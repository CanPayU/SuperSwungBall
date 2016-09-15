using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Extension
{
    public static class ButtonExtension
    {
        public static void EditText(this Button btn, string txt)
        {
            Text textCompoenent = btn.transform.Find("Text").GetComponent<Text>();
            textCompoenent.text = txt;
        }
    }
}
