using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TeamButtonController : MonoBehaviour {

	[SerializeField] private State state;
	[SerializeField] private TextMesh team_name;

	// -- Clic event
	Ray ray;
	RaycastHit hit;
	// --

	private int index;
	private Team[] teams_;

	void Start()
	{
		GetTeams ();
		// -- Nb bouton en fonction Nb team
		int len = Settings.Instance.Default_Team.Count;
		if (len < 3 && state == State.ENABLED)
			gameObject.SetActive (false);
		else if (len < 3 && ((state == State.LEFT && index == (len-1)) || (state == State.RIGHT && index == (0))))
			gameObject.SetActive (false);
		else if (len < 2 && (state == State.LEFT || state == State.RIGHT || state == State.ENABLED))
			gameObject.SetActive (false);
		// --
	}

	private void GetTeams(){
		Dictionary<string, Team> dict = Settings.Instance.Default_Team;
		teams_ = new Team[dict.Count];

		int i = 0; Team selected_t = Settings.Instance.Selected_Team;
		foreach (KeyValuePair<string,Team> team in dict) {
			teams_ [i] = team.Value;
			if (team.Value == selected_t) {
				index = i;
				team_name.text = team.Value.Name;
			}
			i++;
		}
	}

	void OnMouseOver() // event souris dessus
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hit, 100);
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (!hit.Equals(null))
			{
				if (hit.collider == GetComponent<Collider>()) // Collision clic
				{
					var click_state = this.state;
					foreach (var btn in GameObject.FindGameObjectsWithTag ("Menu")) {
						btn.GetComponent<TeamButtonController> ().animateWithStateClic (click_state);
					}
				}
			}
		}
	}


	public void animateWithStateClic(State state){
		switch (state) {
		case State.UP:
			break;//return;
		case State.LEFT:
			index++;
			if (this.state == State.LEFT) {
				GetComponent<Animator> ().Play ("LeftToUp");
				this.state = State.UP;
			} else if (this.state == State.UP) {
				GetComponent<Animator> ().Play ("UpToRight");
				this.state = State.RIGHT;
			} else if (this.state == State.RIGHT) {
				GetComponent<Animator> ().Play ("RightToEnabled");
				this.state = State.ENABLED;
			} else if (this.state == State.ENABLED) {
				GetComponent<Animator> ().Play ("EnabledToLeft");
				this.state = State.LEFT;
			}
			break;
		case State.RIGHT:
			index--;
			if (this.state == State.LEFT){
				GetComponent<Animator> ().Play ("LeftToEnabled");
				this.state = State.ENABLED;
			}else if (this.state == State.UP){
				GetComponent<Animator> ().Play ("UpToLeft");
				this.state = State.LEFT;
			}else if (this.state == State.RIGHT){
				GetComponent<Animator> ().Play ("RightToUp");
				this.state = State.UP;
			}else if (this.state == State.ENABLED){
				GetComponent<Animator> ().Play ("EnabledToRight");
				this.state = State.RIGHT;
			}break;
		default:
			break;
		}
		if (this.state == State.UP) {
			var selected_t = teams_ [index];
			Settings.Instance.Selected_Team = selected_t;
			team_name.text = selected_t.Name;
		}
	}

	public enum State {
		LEFT,
		RIGHT,
		UP,
		ENABLED
	}
}
