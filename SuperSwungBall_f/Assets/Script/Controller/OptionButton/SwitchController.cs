using UnityEngine;
using UnityEngine.UI;
using System.Collections;


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
		Debug.Log (slider.value);
	}
}
