using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;

public class OnClickController : MonoBehaviour {

	public Action OnClicked;
	public Action<int> OnClickedValue;
	public int Value;

	public void OnMouseDown(){
		if (this.OnClicked != null)
			this.OnClicked ();
		if (this.OnClickedValue != null)
			this.OnClickedValue (Value);
	}
}
