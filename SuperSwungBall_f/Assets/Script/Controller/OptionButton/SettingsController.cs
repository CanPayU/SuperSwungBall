using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;


namespace OptionButton
{
    public class SettingsController : MonoBehaviour
    {
		private GameObject panel;
		private float actual_position = 0;

        // Use this for initialization
        void Start()
        {
			panel = transform.Find ("Panel").gameObject;

			var btns = getButtons (SceneManager.GetActiveScene ().buildIndex);
			foreach (ButtonType btn in btns) {
				AddButton (btn);
			}
        }

		private ButtonType[] getButtons(int buildIndex) {
			if (buildIndex < 2)
				return  new ButtonType[0];
			if (buildIndex < 6) {
				var array_mid = new ButtonType[2];
				array_mid [0] = ButtonType.Notification;
				array_mid [1] = ButtonType.Sound;
				return array_mid;
			}
			var array = new ButtonType[4];
			array [0] = ButtonType.Notification;
			array [1] = ButtonType.Sound;
			array [2] = ButtonType.KeyPasse;
			array [3] = ButtonType.Quit;
			return array;
		}

		// ajouter un bouton dans le panel
		public void AddButton(ButtonType type){
			string name = type.Value;
			GameObject btn = Resources.Load("Prefabs/OptionButton/" + name) as GameObject;
			if (btn != null)
				instanciateButton (btn);
			else
				Debug.LogError ("Prefab : \"Prefabs/OptionButton/" + name + "\" introuvable");
		}

		private void instanciateButton(GameObject btnGm) {
			RectTransform rt_panel = (RectTransform)panel.transform;
			Transform btn = Instantiate(btnGm).transform as Transform;
			float btn_heigth = ((RectTransform)btn).sizeDelta.y;

			float new_panel_heigth = rt_panel.offsetMin.y;
			if (actual_position > 0) // Deja un bouton ?
				new_panel_heigth -= (btn_heigth + 5); // pour la marge top
			else
				new_panel_heigth -= (btn_heigth + 7.5f); // pour la marge bottom
			rt_panel.offsetMin = new Vector2 (rt_panel.offsetMin.x, new_panel_heigth);

			btn.SetParent(panel.transform, false);
			this.actual_position -= ((btn_heigth / 2) + 5);
			((RectTransform)btn).anchoredPosition = new Vector2(0, actual_position);
			this.actual_position -= ((btn_heigth / 2));
		}

		public sealed class ButtonType
		{
			public static readonly ButtonType Quit = new ButtonType("Quit");
			public static readonly ButtonType Notification = new ButtonType("Notification");
			public static readonly ButtonType Sound = new ButtonType("Sound");
			public static readonly ButtonType KeyPasse = new ButtonType("KeyPasse");

			private ButtonType(string value)
			{
				Value = value;
			}

			public string Value { get; private set; }
		}
    }
}
