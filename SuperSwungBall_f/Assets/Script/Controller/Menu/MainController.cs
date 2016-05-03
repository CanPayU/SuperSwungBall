using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Menu
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private Text account_username;
        [SerializeField]
        private Text account_score;
        [SerializeField]
        private Text account_phi;

        private Timer time;

        // Use this for initialization
        void Start()
        {
            time = new Timer(60.0F, Inactive);
            time.start();
        }

        // Update is called once per frame
        void Update()
        {
            time.update();
            if (Input.anyKey || Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                time.reset();
                time.start();
            }
        }

        void Inactive()
        {
			FadingManager.Instance.Fade("standing");
        }
    }
}