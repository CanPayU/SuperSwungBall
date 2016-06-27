using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Extension;
using TranslateKit;

namespace OptionButton
{
	public class NotificationController : MonoBehaviour
	{
		private Button btn;
		private NotificationState actualNotificationState;

		private int pressId = 0;

		private string defaultText = "Notification"; //inutile ?
		private Dictionary<string, string> transKeys = new Dictionary<string, string>()
		{
			{"default", "Notification"},
			{"state1", Language.GetValue(TradValues.General.All)},
			{"state2", Language.GetValue(TradValues.General.Private)},
			{"state3", Language.GetValue(TradValues.General.Nothing)}
		};

		// Use this for initialization
		void Start()
		{
			this.btn = gameObject.GetComponent<Button> ();
			actualNotificationState = Settings.Instance.NotificationState;
			if (this.btn != null)
			{
				this.btn.onClick.AddListener(delegate ()
					{
						OnChangeNotificationState();
					});
				updateView();
				this.btn.EditText(this.transKeys["default"]);
			}
		}

		void OnDisable(){
			if (this.btn != null)
				this.btn.EditText(this.transKeys["default"]);
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
				this.btn.EditText(this.transKeys["default"]);
		}

		private void updateView()
		{
			switch (this.actualNotificationState)
			{
			case NotificationState.All:
				this.btn.colors = Colors.Block.Green;
				this.btn.EditText(this.transKeys["state1"]);
				break;
			case NotificationState.Private:
				this.btn.colors = Colors.Block.Orange;
				this.btn.EditText(this.transKeys["state2"]);
				break;
			case NotificationState.Nothing:
				this.btn.colors = Colors.Block.Red;
				this.btn.EditText(this.transKeys["state3"]);
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
