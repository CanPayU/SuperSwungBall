using UnityEngine;
using System.Collections;

public class GoalController : MonoBehaviour {

	[SerializeField]
	private int team_id;

	private GameObject main;

	// Use this for initialization
	void Start () {
		main = GameObject.Find ("Main");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void goal(){
		Game.Instance.goal (team_id);
		MainController controller = main.GetComponent<MainController> ();
		controller.update_score ();
	}
}
