using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class ButtonController : MonoBehaviour
    {
        [SerializeField]
        private string scene;
        private Animator myAnimator;

        void Start()
        {
            myAnimator = transform.GetChild(0).GetComponent<Animator>();
        }

        void OnMouseEnter()
        {
            myAnimator.Play("Action");
        }

        void OnMouseExit()
        {
            myAnimator.Play("Repos");
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Choice")
            {
				FadingManager.Instance.Fade(scene);
            }
        }
    }

}
