using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainController : MonoBehaviour {



    private GameObject[] players;

    private bool annim_started = false;

    float timer = 5.0F;
    Text myGuiText;

    Timer time;

    // Use this for initialization
    void Start () {
        players = GameObject.FindGameObjectsWithTag("Player");
        time = new Timer(5.0F, end_time);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space) && !annim_started)
        {
            start_annim();
        }
        time.update();

    }

    private void end_time()
    {
        time.reset();
        foreach (GameObject player in players)
        {
            Player controller = player.GetComponent<Player>();
            controller.end_move();
        }


        if (annim_started)
        {
            annim_started = false;
            time.start();

            Debug.Log("Start reflexion");
        }
        else
        {
            annim_started = true;
            start_annim();
        }
    }

    private void start_annim()
    {
        Debug.Log("Start Annim  !");
        time.start();
        annim_started = true;
        foreach (GameObject player in players)
        {
            Player controller = player.GetComponent<Player>();
            controller.start_move();
        }
    }

    void OnGUI()
    {

        if ( true)//!annim_started)
        {
            float h = 30;
            float w = 200;
            Rect r = new Rect(0, 0, Screen.width, h);
            Vector2 v = r.center;
            float x = v.x - (w / 2);
            GUI.Box(new Rect(x, 0, w, h), "Timer : " + (time.Time_remaining).ToString("0"));
        }
    }
}
