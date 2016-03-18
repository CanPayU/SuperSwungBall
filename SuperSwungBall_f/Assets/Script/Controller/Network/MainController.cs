using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Network
{
    public class MainController : MonoBehaviour
    {

        [SerializeField]
        private GameObject connection_panel;
        [SerializeField]
        private GameObject loading_panel;
        [SerializeField]
        private GameObject private_panel;

        // Use this for initialization
        void Start()
        {
            if (!User.Instance.is_connected)
            {
                //connection();
                connection_panel.SetActive(true);
            }
            else {
                //SaveLoad.load_settings (); // a supprimer
                loading_panel.SetActive(true);
            }
        }

        public void private_game()
        {
            PhotonNetwork.Disconnect();
            private_panel.SetActive(true);
            loading_panel.SetActive(false);
            connection_panel.SetActive(false);
        }


        public void connect_network()
        {
            loading_panel.SetActive(true);
            private_panel.SetActive(false);
            connection_panel.SetActive(false);
        }

    }
}
