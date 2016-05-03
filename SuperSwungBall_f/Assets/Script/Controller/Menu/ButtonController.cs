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
			if (transform.childCount > 0)
            	myAnimator = transform.GetChild(0).GetComponent<Animator>();
        }

        void OnMouseEnter()
        {
			if (myAnimator != null)
            	myAnimator.Play("Action");
        }

        void OnMouseExit()
		{
			if (myAnimator != null)
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
