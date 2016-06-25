using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace OptionButton
{
    public class MoreController : MonoBehaviour
    {

        private Text text;
        private Button btn;

        private GameObject moreSettings; //inutile ?

        // Use this for initialization
        void Start()
        {
            this.btn = gameObject.GetComponent<Button>();
            this.text = transform.Find("Text").GetComponent<Text>();
            this.moreSettings = Resources.Load("Prefabs/OptionButton/MoreSettings") as GameObject;
            if (this.btn != null)
            {
                this.btn.onClick.AddListener(delegate ()
                    {
                        var gm = GameObject.Instantiate(this.moreSettings);
                        Transform Canvas = GameObject.FindObjectOfType<Canvas>().transform;
                        gm.transform.SetParent(Canvas, false);
                    });
            }
        }
    }
}