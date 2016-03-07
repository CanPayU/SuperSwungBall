using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private GameObject valeur;
    [SerializeField]
    private GameObject zone;

    private GameObject[] menus = new GameObject[4];
    private GameObject[] valeurs = new GameObject[3];
    private GameObject[] zones = new GameObject[2];
    private GameObject[] pointeurs = new GameObject[2];

    private bool clicked;
    private Transform myTransform;
    private Collider myCollider;

    private bool mouseState;
    private GameObject target;
    private GameObject zone_target;
    private Vector3 screenSpace;
    private Vector3 offset;
    RaycastHit hit;

    bool deplacement = false;
    Vector3 arrive;

    private Vector3 target_passe;


    // Variable pour Passe
    private GameObject ball;

    void Start()
    {
        #region creation
        clicked = false;
        myTransform = GetComponent<Transform>();
        myCollider = GetComponent<Collider>();
        //creation Menus
        ajouter_menu(0, new Color(1, 0.5f, 0));
        ajouter_menu(1, new Color(0, 0.5f, 0));
        ajouter_menu(2, new Color(1, 0.2f, 0.7f));
        ajouter_menu(3, new Color(0, 0.5f, 1));
        //creation valeurs
        ajouter_valeur(0);
        ajouter_valeur(1);
        ajouter_valeur(2);
        //creation zones
        ajouter_zone(0, 10, new Color(0.5f, 1, 1), -0.3f);
        ajouter_zone(1, 0, new Color(1, 0.5f, 1), -0.28f);
        //creation pointeurs
        ajouter_pointeur(0, new Color(0.3f, 0.8f, 1f));
        ajouter_pointeur(1, new Color(0.8f, 0.3f, 0.8f));
        #endregion
        // recup de la balle
        ball = GameObject.Find("Ball");

        
        afficher_menu(false);
    }

    void Update()
    {
        #region Reflexion
        if (!deplacement)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                // s'active une seul fois au clic
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                #region aficher/effacer menu
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.collider == myCollider || hit.collider.transform.parent == myTransform)
                    {
                        if (!clicked)
                        {
                            afficher_menu(true);
                        }
                    }
                    else
                    {
                        clicked = false;
                        afficher_menu(false);
                    }
                }
                #endregion
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.collider == myCollider || hit.collider.transform.parent == myTransform)
                    {
                        target = GetClickedObject(out hit);
                        if (target != null)
                        {
                            mouseState = true;
                        }
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                mouseState = false;
            }
            if (mouseState)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Physics.Raycast(ray.origin, ray.direction * 10, out hit);

                target.transform.position = new Vector3(hit.point.x, zone_target.transform.position.y + 0.05f, hit.point.z); // position absolue
                target.transform.LookAt(zone_target.transform);

                update_deplacement(target, zone_target);
            }
        }
        #endregion
        if (deplacement)
        {/*
                myTransform.Translate(Vector3.forward * Time.deltaTime * 5, Space.Self);
                myTransform.position = new Vector3(myTransform.position.x, 0.5f, myTransform.position.z);
                */
                float step = 2.0f * Time.deltaTime;
            Vector3 depart = new Vector3(transform.position.x, 0.5f, transform.position.z); // suppression du bug de hauteur
                transform.position = Vector3.MoveTowards(depart, arrive, step);

            // Au clic, call BallController et declanche passe
            if (Input.GetKey(KeyCode.A))
                {
                    BallController script = ball.GetComponent<BallController>();
                    if(script.Holder == gameObject)
                    {
                        script.trigger_passe(target_passe); // call function
                    }
                }

            
        }
    }
    #region initialisation
    void ajouter_menu(int n, Color c)

    {
        menus[n] = Instantiate(menu, myTransform.position + new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        menus[n].transform.eulerAngles = new Vector3(270, 60 * n + 60, 0);
        menus[n].transform.localScale *= 1.5f;
        menus[n].transform.parent = myTransform;
        menus[n].GetComponent<Renderer>().material.color = c;
        menus[n].name = "menu" + n;
    }
    void ajouter_valeur(int n)
    {
        valeurs[n] = Instantiate(valeur, myTransform.position + new Vector3(0.5f * n - 0.5f, 0, -1.1f), Quaternion.identity) as GameObject;
        valeurs[n].transform.parent = myTransform;
        valeurs[n].GetComponent<Renderer>().material.color = Color.gray;
        valeurs[n].name = "valeur" + n;
    }
    void ajouter_zone(int n, int taille, Color c, float y)

    {
        zones[n] = Instantiate(zone, myTransform.position + new Vector3(0, y, 0), Quaternion.identity) as GameObject;
        zones[n].transform.parent = myTransform;
        zones[n].transform.localScale = new Vector3(taille, 0.05f, taille);
        zones[n].GetComponent<Renderer>().material.color = c;
        zones[n].name = "zone" + n;
    }
    void ajouter_pointeur(int n, Color c)
    {
        pointeurs[n] = Instantiate(zone, myTransform.position + new Vector3(0, -0.23f + 0.5f*n, 0), Quaternion.identity) as GameObject;
        pointeurs[n].transform.parent = myTransform;
        pointeurs[n].transform.localScale = new Vector3(1, 0.05f, 1);
        pointeurs[n].GetComponent<Renderer>().material.color = c;
        pointeurs[n].name = "pointeur" + n;
    }
    #endregion

    public void start_move()
    {
        deplacement = true;
        arrive = pointeurs[0].transform.position;
        target_passe = pointeurs[1].transform.position;
        myTransform.LookAt(arrive);
        afficher_menu(false);
    }

    public void end_move()
    {
        deplacement = false;
        myTransform.rotation = Quaternion.identity;
        pointeurs[0].transform.localPosition = new Vector3(0, 0, 0);
        pointeurs[1].transform.localPosition = new Vector3(0, 0, 0);
    }

    private void afficher_menu(bool afficher)
    {
        for (int i = 0; i < myTransform.childCount; ++i)
        {
            if (myTransform.GetChild(i).tag == "Menu")
            {
                myTransform.GetChild(i).GetComponent<Collider>().enabled = afficher;
                myTransform.GetChild(i).GetComponent<Renderer>().enabled = afficher;
            }
        }
    }
    public void changer_valeur(Color c)
    {
        valeurs[0].GetComponent<Renderer>().material.color = valeurs[1].GetComponent<Renderer>().material.color;
        valeurs[1].GetComponent<Renderer>().material.color = valeurs[2].GetComponent<Renderer>().material.color;
        valeurs[2].GetComponent<Renderer>().material.color = c;
        update_menu();
        update_deplacement(pointeurs[0], zones[0]);
        update_deplacement(pointeurs[1], zones[1]);
    }
    private void update_menu()
    {
        //update zones
        int longueur_deplacement = 10;
        int longueur_passe = 0;
        foreach (GameObject obj in valeurs)
        {
            Color col = obj.GetComponent<Renderer>().material.color;
            if (col == menus[3].GetComponent<Renderer>().material.color)
            {
                longueur_deplacement += 5;
            }
            if (col == menus[2].GetComponent<Renderer>().material.color)
            {
                longueur_passe += 11;
            }
        }
        Destroy(zones[0]);
        Destroy(zones[1]);
        if (longueur_passe < longueur_deplacement)
        {
            ajouter_zone(0, longueur_deplacement, new Color(0.5f, 1, 1), -0.3f);
            ajouter_zone(1, longueur_passe, new Color(1, 0.5f, 1), -0.28f);
        }
        else
        {
            ajouter_zone(0, longueur_deplacement, new Color(0.5f, 1, 1), -0.28f);
            ajouter_zone(1, longueur_passe, new Color(1, 0.5f, 1), -0.3f);
        }
    }

    private void update_deplacement(GameObject pointeur, GameObject zone)
    {
        float distance = Vector3.Distance(pointeur.transform.position, zone.transform.position);
        if (distance > zone.transform.localScale.x / 2)
        {
            float angle = pointeur.transform.eulerAngles.y;
            pointeur.transform.localPosition = new Vector3(-Mathf.Sin(Mathf.Deg2Rad * angle) * zone.transform.localScale.x / 2, -0.23f, -Mathf.Cos(Mathf.Deg2Rad * angle) * zone.transform.localScale.x / 2);
        }
    }
    GameObject GetClickedObject(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            var gm = hit.collider.gameObject;
            if (gm == zones[0] || gm == pointeurs[0])
            {
                target = pointeurs[0];
                zone_target = zones[0];
            }
            else
            {
                if (gm == zones[1] || gm == pointeurs[1])
                {
                    target = pointeurs[1];
                    zone_target = zones[1];
                }
                else
                    target = null; mouseState = false;
            }

        }

        return target;
    }//Drag and drop
}