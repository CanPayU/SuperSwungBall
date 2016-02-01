﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour
{
    private bool _mouseState;
    private GameObject target;
    public Vector3 screenSpace;
    public Vector3 offset;

    private GameObject pointB;

    // Use this for initialization
    void Start()
    {
        pointB = GameObject.Find("PointB");
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(_mouseState);
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hitInfo;
            target = GetClickedObject(out hitInfo);
            if (target != null)
            {
                _mouseState = true;
                screenSpace = Camera.main.WorldToScreenPoint(target.transform.position);
                offset = target.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _mouseState = false;
        }
        if (_mouseState)
        {
            //keep track of the mouse position
            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

            //convert the screen mouse position to world point and adjust with offset
            var curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            //update the position of the object in the world
            target.transform.position = curPosition;
        }
    }


    GameObject GetClickedObject(out RaycastHit hit)
    {
        //GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            var gm = hit.collider.gameObject;
            if (gm.tag == "Player")
            {
                pointB.transform.position = hit.point;
                target = pointB;
            }
            else
                target = null; _mouseState = false;
        }

        return target;
    }
}

















    /*
    private bool drag = false;
    private Transform trans;
    private GameObject PointB;
    RaycastHit hitP;

    public Vector3 screenSpace;
    public Vector3 offset;

    // Use this for initialization
    void Start () {

        PointB = GameObject.Find("pointB");
        trans = PointB.GetComponent<Transform>();
        hitP.point = PointB.transform.position;
    }
	
	// Update is called once per frame
	void Update () {


        if (Input.GetKeyDown(KeyCode.Mouse0)) // clic droit
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // recuperation de la pos
            if (Physics.Raycast(ray, out hit, 100)) // sur un objet ?
            {
                GameObject gm = hit.transform.gameObject;

                if (gm.tag == "pointFleche") // sur un pointFleche ?
                {
                    Debug.Log("onPoint");
                    drag = true;
                    trans.LookAt(hit.point);
                    hitP = hit;

                    screenSpace = Camera.main.WorldToScreenPoint(PointB.transform.position);
                    offset = PointB.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, screenSpace.z));
                }
                else
                {
                    drag = false;
                }
            }
            else
            {
                drag = false;
            }
        }


        if (Input.GetMouseButton(0) && drag)
        {
            Debug.Log("appuiyer");

            Vector2 screenPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector3 convertedGUIPos = new Vector3(GUIUtility.ScreenToGUIPoint(screenPos).x, 0f, GUIUtility.ScreenToGUIPoint(screenPos).y);

            var curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0F);

            hitP.point = Camera.main.ScreenToWorldPoint(curScreenSpace);

            trans.position = hitP.point;
        }


    }
    */
