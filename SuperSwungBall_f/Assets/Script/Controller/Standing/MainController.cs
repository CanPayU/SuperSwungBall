using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Standing
{
    public class MainController : MonoBehaviour
    {

        [SerializeField]
        private string scene;

		// a supp
		[SerializeField]
		private GameObject music;

        // Use this for initialization
        void Start()
        {
			SaveLoad.save_setting ();
			//SaveLoad.load_settings ();
			SaveLoad.load_user ();



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

