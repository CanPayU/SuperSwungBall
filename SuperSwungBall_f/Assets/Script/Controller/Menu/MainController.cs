using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Boomlagoon.JSON;

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
			checkChallengeCompleted ();
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

		private void checkChallengeCompleted(){
			JSONArray challenges = ApplicationModel.ChallengeCompleted;

			if (challenges == null || challenges.Length < 1)
				return;

			JSONObject obj = challenges [0].Obj;
			challenges.Remove (0);

			string swName = obj.GetObject ("swungMan").GetString("name");
			string text = obj.GetString ("text");
			string title = "Nouveau défi débloqué !";
			string content = text + "\n" + "Vous avez obtenu un nouveau SwungMan : " + swName + "\n" + "Rendez-vous dans la boutique pour en savoir plus !";
			Notification.SimpleAlert (title, content, force: true, completion: () => {
				checkChallengeCompleted();
			});
		}
    }
}