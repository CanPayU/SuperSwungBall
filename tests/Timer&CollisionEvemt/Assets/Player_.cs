using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Player_ : MonoBehaviour {

    AudioSource audio;
    GameObject myPlayer;

    // Use this for initialization
    void Start ()
    {
        audio = GetComponent<AudioSource>();
        Debug.Log(audio.isPlaying);
        myPlayer = GameObject.Find("Player");
    }
	
	// Update is called once per frame
	void Update () {


        Vector3 translation = new Vector3(0, 0);

        if (Input.GetButton("Horizontal"))
        {
            float speed = Input.GetAxisRaw("Horizontal");
            translation.x = (speed/10);
        }
        if (Input.GetButton("Vertical"))
        {
            float speed = Input.GetAxisRaw("Vertical");
            translation.y = (speed / 10);
        }
        
        myPlayer.transform.Translate(translation);

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
        }
    }




    void OnCollisionEnter (Collision collision)
    {

        Debug.Log("OnCollisionEnter ! " + collision.relativeVelocity.magnitude);
        
        if (collision.relativeVelocity.magnitude > 0)
        {
            audio.Play();
        }

    }
}
