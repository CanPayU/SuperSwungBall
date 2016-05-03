using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace OptionButton
{
	public class NotificationController : MonoBehaviour
	{
		private Button btn;
		private NotificationState actual_NotificationState;

		// Use this for initialization
		void Start()
		{
			this.btn = gameObject.GetComponent<Button> ();
			actual_NotificationState = Settings.Instance.NotificationState;
			if (this.btn != null)
			{
				this.btn.onClick.AddListener(delegate ()
					{
						OnChangeNotificationState();
					});
				Update_ColorAndText();
			}
		}

		private void OnChangeNotificationState()
		{
			actual_NotificationState = Next<NotificationState>(actual_NotificationState);
			Settings.Instance.NotificationState = actual_NotificationState;
			SaveLoad.save_setting ();
			Update_ColorAndText();
		}

		private void Update_ColorAndText()
		{
			Text text = this.btn.transform.Find("Text").GetComponent<Text>();
			switch (actual_NotificationState)
			{
			case NotificationState.All:
				this.btn.image.color = new Color(0f / 255f, 173f / 255f, 0f / 255f);
				text.text = "Tout";
				break;
			case NotificationState.Private:
				this.btn.image.color = new Color(191f / 255f, 94f / 255f, 0f / 255f);
				text.text = "Prive";
				break;
			case NotificationState.Nothing:
				this.btn.image.color = new Color(174f / 255f, 17f / 255f, 17f / 255f);
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
