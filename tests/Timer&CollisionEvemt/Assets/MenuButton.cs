﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    [SerializeField]
    private string scene;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == "MenuController")
        {
            StartCoroutine(ChangeLevel());
        }
    }
    
    
    IEnumerator ChangeLevel()
    {
        float fadeTime = GameObject.Find("GM").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scene);
    }
    
}