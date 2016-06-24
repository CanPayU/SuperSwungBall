using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundController : MonoBehaviour
{
    private List<string> paths;

    private new AudioSource audio;
    private AudioClip clip;

    private System.Random rand = new System.Random();


    // Use this for initialization
    void Start()
	{
		this.paths = new List<string>();
		foreach (var item in Game.Instance.Teams)
		{
			foreach (var sound in item.Value.Sounds)
			{
				this.paths.Add(sound);
			}
		}

		//var team = Settings.Instance.Default_Team["psg"];
		//paths = team.Sounds;
		this.audio = GetComponent<AudioSource> ();

		if (paths.Count > 0) {
			StartCoroutine(trigger_audio());
		}
    }

    IEnumerator trigger_audio()
    {
		int alea = rand.Next(paths.Count);
        string path = paths[alea];
        this.clip = Resources.Load(path) as AudioClip;
		this.audio.clip = this.clip;
        this.audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
        StartCoroutine(trigger_audio());
    }
}
