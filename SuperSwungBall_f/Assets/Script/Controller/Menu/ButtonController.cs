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
        private Material[] myMaterial;
        private Color ENTER = new Color(1f, 0.8f, 0f); // Couleur lors du passage de la sourie
        private Color EXIT = new Color(1f, 0.5f, 0f); // Couleur par défaut

        void Start()
        {
            if (transform.childCount > 0)
                myAnimator = transform.GetChild(0).GetComponent<Animator>();
            findMaterial(name);
            ChangeColor(EXIT);
        }

        void OnMouseEnter()
        {
            if (myAnimator != null)
            {
                ChangeColor(ENTER);
                myAnimator.Play("Action");
            }
        }

        void OnMouseExit()
        {
            if (myAnimator != null)
            {
                ChangeColor(EXIT);
                myAnimator.Play("Repos");
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "Choice")
            {
                if (this.scene == "SoloGame")
                    ApplicationModel.TypeToInstanciate = GameType.Solo;
                FadingManager.Instance.Fade(scene);
            }
        }

        private void findMaterial(string name)
        {
            switch (name)
            {
                case "Button_multi":
                    myMaterial = new Material[6];
                    myMaterial[1] = transform.FindChild("animationReposMulti").FindChild("Ball").GetComponent<Renderer>().material;
                    myMaterial[2] = transform.FindChild("animationReposMulti").FindChild("Cube").FindChild("Cube_MeshPart0").GetComponent<Renderer>().material;
                    myMaterial[3] = transform.FindChild("animationReposMulti").FindChild("Cube").FindChild("Cube_MeshPart1").GetComponent<Renderer>().material;
                    myMaterial[4] = transform.FindChild("animationReposMulti").FindChild("Cube_001").FindChild("Cube_001_MeshPart0").GetComponent<Renderer>().material;
                    myMaterial[5] = transform.FindChild("animationReposMulti").FindChild("Cube_001").FindChild("Cube_001_MeshPart1").GetComponent<Renderer>().material;
                    break;
                case "Button_Tuto":
                    myMaterial = new Material[4];
                    myMaterial[1] = transform.FindChild("animationActionTuto").FindChild("Cube").FindChild("Cube_MeshPart0").GetComponent<Renderer>().material;
                    myMaterial[2] = transform.FindChild("animationActionTuto").FindChild("Cube").FindChild("Cube_MeshPart1").GetComponent<Renderer>().material;
                    myMaterial[3] = transform.FindChild("animationActionTuto").FindChild("Sphere").GetComponent<Renderer>().material;
                    break;
                case "Button_boutique":
                    myMaterial = new Material[11];
                    for (int i = 1; i < 10; i++)
                        myMaterial[i] = transform.FindChild("BoutonBoutique").FindChild("p" + i).GetComponent<Renderer>().material;
                    myMaterial[10] = transform.FindChild("BoutonBoutique").FindChild("armature").FindChild("ventre").FindChild("buste").FindChild("tete").FindChild("casqueDefaut").GetComponent<Renderer>().material;
                    break;
                case "Button_create_team":
                    myMaterial = new Material[3];
                    myMaterial[1] = transform.FindChild("BoutonTeam").FindChild("armature").FindChild("ventre").FindChild("buste").FindChild("tete").FindChild("casqueDefaut").GetComponent<Renderer>().material;
                    myMaterial[2] = transform.FindChild("BoutonTeam").FindChild("Cube").GetComponent<Renderer>().material;
                    break;
                default:
                    myMaterial = new Material[1];
                    break;
            }
            myMaterial[0] = transform.FindChild("Text").GetComponent<Renderer>().material;
        }

        private void ChangeColor(Color c)
        {
            foreach (Material m in myMaterial)
            {
                m.color = c;
            }
        }
    }
}