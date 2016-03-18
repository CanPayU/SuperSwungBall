using UnityEngine;
using System.Collections;

public class AnimationButtonControler : MonoBehaviour
{

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

}
