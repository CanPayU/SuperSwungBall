using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace OptionButton
{
	public class NotificationController : MonoBehaviour
	{
		private Text text;
		private Button btn;
		private NotificationState actualNotificationState;

		private int pressId = 0;
		private string defaultText = "Notification";

		// Use this for initialization
		void Start()
		{
			this.btn = gameObject.GetComponent<Button> ();
			this.text = transform.Find("Text").GetComponent<Text>();
			actualNotificationState = Settings.Instance.NotificationState;
			if (this.btn != null)
			{
				this.btn.onClick.AddListener(delegate ()
					{
						OnChangeNotificationState();
					});
				updateView();
				this.text.text = this.defaultText;
			}
		}

		void OnDisable(){
			if (this.text != null)
				this.text.text = this.defaultText;
		}

		private void OnChangeNotificationState()
		{
			this.pressId++;
			this.actualNotificationState = Next<NotificationState>(actualNotificationState);
			Settings.Instance.NotificationState = actualNotificationState;
			SaveLoad.save_setting ();
			updateView();
			StartCoroutine(defaultSetUp (this.pressId));
		}

		private IEnumerator defaultSetUp(int id){
			yield return new WaitForSeconds (5);
			if (id == this.pressId)
				this.text.text = this.defaultText;
		}

		private void updateView()
		{
			switch (this.actualNotificationState)
			{
			case NotificationState.All:
				this.btn.colors = Colors.Block.Green;
				text.text = "Tout";
				break;
			case NotificationState.Private:
				this.btn.colors = Colors.Block.Orange;
				text.text = "Prive";
				break;
			case NotificationState.Nothing:
				this.btn.colors = Colors.Block.Red;
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
