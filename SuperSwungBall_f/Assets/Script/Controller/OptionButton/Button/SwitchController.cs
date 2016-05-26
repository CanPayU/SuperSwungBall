using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace OptionButton {

	public class SwitchController : MonoBehaviour {

		private Slider slider;
		private Image background;
		private Image round;

		void Awake(){
			this.background = transform.Find ("Background").GetComponent<Image> ();
			this.round = transform.Find ("Handle Slide Area").Find ("Handle").GetComponent<Image> ();
			this.slider = GetComponent<Slider> ();
			this.slider.onValueChanged.AddListener (delegate {OnValueChange ();});
			setUpColor ();
		}

		private void OnValueChange(){
			setUpColor ();
		}
		
		private void setUpColor(){
			if (slider.value <= 0) {
				this.background.color = Colors.Normal.Red;
				this.round.color = Colors.Pressed.Red;
			} else {
				this.background.color = Colors.Normal.Green;
				this.round.color = Colors.Pressed.Green;
			}
		}

		public bool Value {
			get { return slider.value == 1; }
			set { slider.value = value ? 1 : 0; }
		}

		public void AddListener(UnityAction<bool> action) {
			this.slider.onValueChanged.AddListener (delegate {
				action (this.Value);
			});
		}
	}
}