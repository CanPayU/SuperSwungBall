using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;


namespace OptionButton {
	
	public class SoundController : MonoBehaviour {

		private Text text;
		private Button btn;
		private SoundState actualSoundState;

		private int pressId = 0;
		private string defaultText = "Son";

		// Use this for initialization
		void Start()
		{
			this.btn = gameObject.GetComponent<Button> ();
			this.text = transform.Find("Text").GetComponent<Text>();
			actualSoundState = Settings.Instance.SoundState;
			if (this.btn != null)
			{
				this.btn.onClick.AddListener(delegate ()
					{
						OnChangeSoundState();
					});
				updateView();
				this.text.text = this.defaultText;
			}
		}

		void OnDisable(){
			if (this.text != null)
				this.text.text = this.defaultText;
		}

		private void OnChangeSoundState()
		{
			this.pressId++;
			actualSoundState = Next<SoundState>(actualSoundState);
			Settings.Instance.SoundState = actualSoundState;
			SaveLoad.save_setting ();
			updateView();
			StartCoroutine(defaultSetUp (this.pressId));
		}

		private IEnumerator defaultSetUp(int id){
			yield return new WaitForSeconds (5);
			if (id == this.pressId)
				this.text.text = this.defaultText;
		}

		private void updateView(){
			switch (this.actualSoundState)
			{
			case SoundState.All:
				this.btn.colors = Colors.Block.Green;
				this.text.text = "Tout";
				AudioListener.pause = false;
				break;
			case SoundState.Effect:
				this.btn.colors = Colors.Block.Orange;
				this.text.text = actualSoundState.ToString();
				break;
			case SoundState.Musique:
				this.btn.colors = Colors.Block.Orange;
				this.text.text = actualSoundState.ToString();
				break;
			case SoundState.Nothing:
				this.btn.colors = Colors.Block.Red;
				this.text.text = "Aucun";
				AudioListener.pause = true;
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


