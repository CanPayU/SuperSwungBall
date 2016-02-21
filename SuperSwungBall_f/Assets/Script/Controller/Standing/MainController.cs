using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Standing
{
    public class MainController : MonoBehaviour
    {

        [SerializeField]
        private string scene;

        // Use this for initialization
        void Start()
        {
			bool b = SaveLoad.load_user ();
			Debug.Log("Succes Load User :"+b);
			if (b) {
				Debug.Log ("Username : "+User.Instance.username);
			}
        }

        // Update is called once per frame
        void Update()
        {

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
}

