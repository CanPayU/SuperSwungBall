using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace Menu
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField]
        private Button notificationState;

        private NotificationState actual_NotificationState;

        // Use this for initialization
        void Start()
        {
            actual_NotificationState = Settings.Instance.NotificationState;
            if (notificationState != null)
            {
                notificationState.onClick.AddListener(delegate ()
                {
                    OnChangeNotificationState();
                });
                Update_ColorAndText();
            }
        }

        public void Fade(string scene = "menu")
        {
            FadingManager.I.Fade(scene);
        }

        private void OnChangeNotificationState()
        {
            actual_NotificationState = Next<NotificationState>(actual_NotificationState);
            Settings.Instance.NotificationState = actual_NotificationState;
            Update_ColorAndText();
        }

        private void Update_ColorAndText()
        {
            Text text = notificationState.transform.Find("Text").GetComponent<Text>();
            switch (actual_NotificationState)
            {
                case NotificationState.All:
                    notificationState.image.color = new Color(0f / 255f, 173f / 255f, 0f / 255f);
                    text.text = "Tout";
                    break;
                case NotificationState.Private:
                    notificationState.image.color = new Color(191f / 255f, 94f / 255f, 0f / 255f);
                    text.text = "Prive";
                    break;
                case NotificationState.Nothing:
                    notificationState.image.color = new Color(174f / 255f, 17f / 255f, 17f / 255f);
                    text.text = "Aucune";
                    break;
                default:
                    break;
            }
        }

        private static T Next<T>(T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));
            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }
}
