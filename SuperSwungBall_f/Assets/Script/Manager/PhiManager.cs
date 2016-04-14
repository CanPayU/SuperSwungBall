using UnityEngine;
using System.Collections;

public class PhiManager : MonoBehaviour {

	private static PhiManager instance = new PhiManager();

	private GameObject more_panel;

	private int phi;


	void Start(){
		instance.more_panel = Resources.Load("Prefabs/Setting/MorePhi") as GameObject;
		Debug.Log (more_panel);
		HTTP.SyncPhi (true, (success) => {
			Debug.Log(success + " - Phi Getted");
		});
	}
		
	public void More(){
		Debug.Log (more_panel);
		GameObject gm = Instantiate (instance.more_panel);
		Transform Canvas = GameObject.FindObjectOfType<Canvas>().transform;
		gm.transform.SetParent (Canvas, false);
	}

	public void Add(int value){
		phi += value;
		HTTP.SyncPhi (false, (success) => {
			Debug.Log(success + " - Phi Setted");
		}, phi);
	}

	public int Phi {
		get { return phi; }
		set { phi = value; }
	}
	public static PhiManager I {
		get { return instance; }
	}
}
