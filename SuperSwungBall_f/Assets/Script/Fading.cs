﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour
{

    /*
	 * 
	 * 			CE SCRIPT A ÉTÉ MIS À JOUR DANS FadingManager.cs
	 * 
	 * 	==> Script/Controller/Standing/FadingManager.cs
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	*/

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;
    public string scene = "menu";

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return fadeSpeed;
    }

    void start()
    {
        SceneManager.sceneLoaded += (scene, loadingMode) =>
        {
            StartCoroutine(ChangeLevel());
        };
    }
    /*
    void OnLevelWasLoaded(int level)
    {
        StartCoroutine(ChangeLevel());
    }
    */
    IEnumerator ChangeLevel()
    {
        float fadeTime = GameObject.Find("GM_Fade").GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

    public void Fade()
    {
        StartCoroutine(ChangeScene());
    }


    IEnumerator ChangeScene()
    {
        float fadeTime = BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scene);
    }
}
