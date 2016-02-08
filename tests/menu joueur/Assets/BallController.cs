using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {


    private GameObject holder; // Joueur possedant

    private Vector3 target; // pos de la passe

    private bool passed = false; // passe en cours ?


	// Use this for initialization
	void Start () {
        // initialisation : Parent = captain
        holder = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (passed)
        {
            if (transform.position == target) // Arriver au bout de la passe ?
                passed = false;
            transform.position = Vector3.Lerp(transform.position, target, 0.1F); // mouvement 
        }
    }

    public void trigger_passe(Vector3 target_) // declenchement de la passe
    {
        this.target = target_; // cible du deplacement
        gameObject.transform.parent = null;
        passed = true;
    }

    
    void OnTriggerEnter(Collider other)
    {
        GameObject gmCol = other.gameObject; // gm touche
        if (gmCol.CompareTag("Player"))
        {
            passed = false;
            holder = gmCol; // devient le parent
            gameObject.transform.SetParent(holder.transform);
            transform.localPosition = new Vector3(0, 0.8f,0); // se met sur lui
        }
    }


    #region Getter/Setter
    public GameObject Holder
    {
        get
        {
            return holder;
        }
    }
    #endregion

}
