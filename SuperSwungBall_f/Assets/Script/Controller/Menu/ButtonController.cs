using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField]
        private string scene;

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Choice")
            {
				FadingManager.Instance.Fade(scene);
            }
        }
    }

}
