using UnityEngine;
using System.Collections;

public class TeamButtonController : MonoBehaviour {


	[SerializeField]
	private State state;
	//evite les "GetComponent<>"
	Color myColor;
	Collider myCollider;

	//Clic event
	Ray ray;
	RaycastHit hit;

	void Start()
	{
		myColor = GetComponent<Renderer>().material.color;
		myCollider = GetComponent<Collider>();
		transform.TransformPoint(1, 0, 0);
	}

	void OnMouseEnter() // event souris entre
	{
		//transform.
		transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
	}

	void OnMouseOver() // event souris dessus
	{
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out hit, 100);
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (!hit.Equals(null))
			{
				if (hit.collider == myCollider) // Collision clic
				{
					Debug.Log ("Cliced : "+GameObject.FindGameObjectsWithTag ("Menu").Length + " -- " + this.state);
					var click_state = this.state;
					foreach (var btn in GameObject.FindGameObjectsWithTag ("Menu")) {
							btn.GetComponent<TeamButtonController> ().animateWithStateClic (click_state);
					}

					transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
				}
			}
		}
		if(Input.GetKeyUp(KeyCode.Mouse0))
		{
			transform.localScale += new Vector3(0.02f, 0.02f, 0.02f);
		}
	}

	void OnMouseExit() // event souris quitte
	{
		transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
	}


	public void animateWithStateClic(State state){
		switch (state) {
		case State.UP:
			break;//return;
		case State.LEFT:
			if (this.state == State.LEFT) {
				GetComponent<Animator> ().Play ("LeftToUp");
				this.state = State.UP;
			} else if (this.state == State.UP) {
				GetComponent<Animator> ().Play ("UpToRight");
				this.state = State.RIGHT;
			} else if (this.state == State.RIGHT) {
				GetComponent<Animator> ().Play ("RightToEnabled");
				this.state = State.ENABLED;
				//GetComponent<MeshRenderer> ().enabled = false;
			} else if (this.state == State.ENABLED) {
				//GetComponent<MeshRenderer> ().enabled = true;
				GetComponent<Animator> ().Play ("EnabledToLeft");
				this.state = State.LEFT;
			}
			break;
		case State.RIGHT:
			if (this.state == State.LEFT){
				//GetComponent<MeshRenderer> ().enabled = false;
				GetComponent<Animator> ().Play ("LeftToEnabled");
				this.state = State.ENABLED;
			}else if (this.state == State.UP){
				GetComponent<Animator> ().Play ("UpToLeft");
				this.state = State.LEFT;
			}else if (this.state == State.RIGHT){
				GetComponent<Animator> ().Play ("RightToUp");
				this.state = State.UP;
			}else if (this.state == State.ENABLED){
				//GetComponent<MeshRenderer> ().enabled = true;
				GetComponent<Animator> ().Play ("EnabledToRight");
				this.state = State.RIGHT;
			}break;
		default:
			break;
		}
	}


	public enum State {
		LEFT,
		RIGHT,
		UP,
		ENABLED
	}
}
