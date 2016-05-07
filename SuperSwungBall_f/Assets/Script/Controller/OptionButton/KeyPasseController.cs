using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace OptionButton {

	public class KeyPasseController : MonoBehaviour {

		private Text text;
		private Button btn;
		private KeyCode actualKeyCode;
		private KeyboardAction action = KeyboardAction.Passe;
		private Animator animator;

		private int pressId = 0;
		private string defaultText = "Touche Passe";
		private bool listening = false;

		// Use this for initialization
		void Start()
		{
			this.animator = GetComponent<Animator>();
			this.btn = GetComponent<Button> ();
			this.text = transform.Find("Text").GetComponent<Text>();
			this.actualKeyCode = Settings.Instance.Keyboard[this.action];
			if (this.btn != null)
			{
				this.btn.onClick.AddListener(delegate ()
					{
						OnChange();
					});
				this.text.text = this.defaultText;
			}
		}

		void OnDisable(){
			if (this.text != null)
				this.text.text = this.defaultText;
			this.listening = false;
		}

		void OnGUI(){
			if (!this.listening)
				return;
			Event e = Event.current;
			if (e.isKey && e.keyCode != KeyCode.None) {
				endListenning (e.keyCode);
			}
		}

		private void endListenning(KeyCode code){
			this.animator.Play ("Empty");
			this.listening = false;
			if (code != KeyCode.None) {
				this.actualKeyCode = code;
				this.text.text = actualKeyCode.ToString ();
				Settings.Instance.UpdateKeyboard(this.action, this.actualKeyCode);
				SaveLoad.save_setting ();
			}
			else 
				this.text.text = "Annule";
			this.pressId++;
			StartCoroutine (defaultSetUp (this.pressId));
		}

		private void OnChange()
		{
			this.listening = !this.listening;
			if (!this.listening) {
				endListenning (KeyCode.None);
			} else {
				this.animator.Play ("LoadColor");
				text.text = "Synchro";
			}
		}

		private IEnumerator defaultSetUp(int id){
			yield return new WaitForSeconds (5);
			if (id == this.pressId)
				this.text.text = this.defaultText;
		}
	}
}
