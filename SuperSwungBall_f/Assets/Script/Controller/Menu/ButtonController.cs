using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ButtonController : MonoBehaviour
    {

        [SerializeField]
        private string scene;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.name == "Choice")
            {
                Debug.Log(scene);
				FadingManager.I.Fade (scene);
                //StartCoroutine(ChangeLevel());
            }
        }
    }

}
