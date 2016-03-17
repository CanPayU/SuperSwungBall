using UnityEngine;
using System.Collections;

public class StadiumSwitchController : MonoBehaviour {

	[SerializeField]
	private GameObject actual_stadium;

	private GameObject[] stadiums;
	private int index = 0;
	private int len;

	// Use this for initialization
	void Start () {
		stadiums = Resources.LoadAll<GameObject>("Prefabs/Stadium");
		len = stadiums.Length;

		string act_name = Settings.Instance.Selected_Stadium; int i = 0;
		foreach (var stadium in stadiums) {
			if (stadium.name == act_name) {
				actual_stadium = stadium; index = i; break;
			} i++;
		}

		// -- Instaciate stadium
		Vector3 pos = actual_stadium.transform.position;
		Quaternion rot = actual_stadium.transform.rotation;
		GameObject gm = Instantiate (actual_stadium, pos, rot) as GameObject;
		gm.name = "Stadium_"+index;
		// --
	}
	
	public void SwitchLeft(){
		if (index > 0)
			index--;
		else
			index = len - 1;
		
		actual_stadium = stadiums [index];
		instanciate_actual_stadium ();
	}
	public void SwitchRight(){
		if (index < len-1)
			index++;
		else
			index = 0;

		actual_stadium = stadiums [index];
		instanciate_actual_stadium ();
	}

	private void instanciate_actual_stadium(){
		Debug.Log ("Instanciate : " + actual_stadium.name);
		GameObject stadium = GameObject.FindGameObjectWithTag ("Stadium");
		if(stadium != null)
			StartCoroutine(Exit(stadium));

		Vector3 pos = actual_stadium.transform.position;
		Quaternion rot = actual_stadium.transform.rotation;

		GameObject gm = Instantiate (actual_stadium, pos, rot) as GameObject;
		gm.name = "Stadium_"+index;
		gm.GetComponent<Animator> ().Play ("Enter_"+gm.name);
		Settings.Instance.Selected_Stadium = gm.name;
	}

	private IEnumerator Exit(GameObject gm)
	{
		Debug.Log ("exit : " + gm.name);
		gm.GetComponent<Animator> ().Play ("Exit_"+gm.name);
		yield return new WaitForSeconds(1);
		Destroy(gm);
	}

}
