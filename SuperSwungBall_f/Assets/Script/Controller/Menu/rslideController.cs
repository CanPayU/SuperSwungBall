using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class rslideController : MonoBehaviour {

	[SerializeField]
	private Dropdown dropdown;
	private Team[] teams_;

	// Use this for initialization
	void Start () {

		Dictionary<string, Team> dict = Settings.Instance.Default_Team;
		teams_ = new Team[dict.Count];
		dropdown.options.Clear ();


		dropdown.value = 0;
		int i = 0; Team selected_t = Settings.Instance.Selected_Team;
		foreach (KeyValuePair<string,Team> team in dict) {
			dropdown.options.Add (new Dropdown.OptionData() {text=team.Value.Name});
			teams_ [i] = team.Value;
			if(team.Value == selected_t)
				dropdown.value = i;
			i++;
		}

		dropdown.captionText.text = teams_ [dropdown.value].Name;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			GameObject objt = EventSystem.current.currentSelectedGameObject;
			if (objt == null)
				gameObject.SetActive (false);
		}
	}

	public void OnChangeValue(){
		Settings.Instance.Selected_Team = teams_ [dropdown.value];
		SaveLoad.save_setting ();
	}
}
