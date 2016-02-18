using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour {

    [SerializeField]
    private string scene;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKey)
        {
            StartCoroutine(ChangeLevel());
        }
    }
    
    
    IEnumerator ChangeLevel()
    {
        float fadeTime = GameObject.Find("GM_Fade").GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scene);
    }
}
