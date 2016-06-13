using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using TranslateKit;

namespace OptionButton.MoreSettings {

	public class LanguageController : MonoBehaviour {

		private SwitchController switchReplay;
		private Button fr;
		private Button en;

		// Use this for initialization
		void Start () {
			//Transform content = transform.Find ("Scroll View").Find ("Viewport").Find ("Content");
			this.fr = transform.Find ("Container").Find ("Language").Find("FR").GetComponent<Button>();
			this.fr.onClick.AddListener (delegate() { OnSelectLanguage (AvailableLanguage.FR); });
			this.en = transform.Find ("Container").Find ("Language").Find("EN").GetComponent<Button>();
			this.en.onClick.AddListener (delegate() { OnSelectLanguage (AvailableLanguage.EN); });

			switch (Settings.Instance.SelectedLanguage.Value) {
			case "EN":
				this.en.interactable = false;
				break;
			case "FR":
				this.fr.interactable = false;
				break;
			}

		}

		private void OnSelectLanguage(AvailableLanguage language){
			Settings.Instance.SelectedLanguage = language;
			SaveLoad.save_setting ();
			Debug.Log ("Fade");
			FadingManager.Instance.Fade ("standing");
		}
	}
}