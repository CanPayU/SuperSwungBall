using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Standing
{
    public class ConnectionController : MonoBehaviour
    {
        [SerializeField]
        private InputField username;
        [SerializeField]
        private InputField password;
        [SerializeField]
        private Button connect;
        [SerializeField]
        private Button withoutPass;

        private bool validated = false;

        // Use this for initialization
        void Start()
        {
            username.onValueChanged.AddListener(delegate
            {
                OnChange(true);
            });
            password.onValueChanged.AddListener(delegate
            {
                OnChange(false);
            });
			username.Select ();
        }

		void Update(){
			if (Input.GetKeyDown (KeyCode.Tab)) {
				Debug.Log ("Press Tab");
				if (username.isFocused)
					password.Select ();
				else if (password.isFocused)
					check_login ();
			}
		}

        private void OnChange(bool all)
        {
            if (this.validated)
			{
				connect.colors = Colors.Block.Blue;
				if (all) 
					withoutPass.colors = Colors.Block.Blue;
                this.validated = false;
            }
        }

        public void hors_ligne()
        {
            this.validated = true;
            connect.interactable = false;
            User.Instance.email = "email@email.com";
            User.Instance.id = 1;
            User.Instance.is_connected = true;
            User.Instance.phi = 500000;
            User.Instance.score = 0;
            User.Instance.username = "Test";
            SaveLoad.save_user();
            FadingManager.Instance.Fade();
        }

        public void check_login()
        {
            this.validated = true;
			connect.interactable = false;
			Debug.Log ("false");
            HTTP.Authenticate(username.text, password.text, (success) =>
            {
                if (success) {
                    SaveLoad.save_user();
					FadingManager.Instance.Fade();
                }
                else {
					connect.interactable = true;
					connect.colors = Colors.Block.Red;
                }
            });
        }

        public void OnConnectWithDevice()
        {
            withoutPass.interactable = false;
            HTTP.AuthDeviceAsk(username.text, (success_ask) =>
            {
                if (success_ask)
					Notification.Text("Code de confirmation", "Entrez le code recus sur votre mobile", force: true, completion: (text) =>
                    {
						HTTP.AuthDeviceReply(username.text, text, (success_reply) =>
                        {
                            if (success_reply)
                            {
                                SaveLoad.save_user();
								FadingManager.Instance.Fade();
                            }
                            else
                            {
                                withoutPass.interactable = true;
                                ColorBlock cb = connect.colors;
                                cb.normalColor = new Color(229f / 255f, 76f / 255f, 76f / 255f);
                                withoutPass.colors = cb;
                            }
                        });
                    });
                else
                {
                    withoutPass.interactable = true;
                    ColorBlock cb = connect.colors;
                    cb.normalColor = new Color(229f / 255f, 76f / 255f, 76f / 255f);
                    withoutPass.colors = cb;
                }
            });
        }

        public void LinkInscription()
        {
            Application.OpenURL("http://ssb.shost.ca/register/");
        }
    }
}

