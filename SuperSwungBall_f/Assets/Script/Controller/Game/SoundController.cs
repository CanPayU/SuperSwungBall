using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	private PhotonPlayer local_player;
	private Team team;
	private string[] paths;

	private new AudioSource audio; // not used ?
	private AudioClip clip; // not used ?

	private System.Random rand = new System.Random();


	// Use this for initialization
	void Start () {
		local_player = PhotonNetwork.player;
		team = Game.Instance.Teams [local_player.ID];
		//team = Settings.Instance.Default_Team["psg"];
		paths = team.Sounds;
		audio = GetComponent<AudioSource> ();

		if(paths.Length > 0)
			StartCoroutine(trigger_audio ());


	}



	IEnumerator trigger_audio() {
		int alea = rand.Next (paths.Length);
		AudioSource audio = GetComponent<AudioSource>();
		string path = paths [alea];
		clip = Resources.Load(path) as AudioClip;

		audio.Play();
		yield return new WaitForSeconds(audio.clip.length);
		StartCoroutine(trigger_audio ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
