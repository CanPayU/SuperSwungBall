using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Camera_ : MonoBehaviour {


    float timer = 5.0F;
    Text myGuiText;
    AudioSource sound;

    Timer time;

    // Use this for initialization
    void Start () {
        time = new Timer(5.0F, end);
        sound = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("return"))
        {
            Debug.Log("Start Timer !");
            time.start();
        }
        time.update();

    }

    private void end()
    {
        Debug.Log("Timer End");
        sound.Play();
    }


    void OnGUI()
    {
        float h = 30;
        float w = 200;
        Rect r = new Rect(0, 0, Screen.width, h);
        Vector2 v = r.center;
        float x = v.x - (w / 2);
        GUI.Box(new Rect(x, 0, w, h), "Timer : " + (time.Time_remaining).ToString("0"));
    }
}
