using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Network
{
    public class LoadingController : MonoBehaviour
    {
        private float timer;
        [SerializeField]
        private Text loading;
        private string loading_text = "Chargement ";

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void OnGUI()
        {
            timer += Time.deltaTime;
            int nb_point = (int)(timer % 4) + 1;
            string points = "";
            for (int i = 0; i < nb_point - 1; i++)
            {
                points += ".";
            }
            loading.text = loading_text + points;
        }
    }
}