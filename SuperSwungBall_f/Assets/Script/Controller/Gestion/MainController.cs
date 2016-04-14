using UnityEngine;
using System.Collections;

// -- TEST
using UnityEngine.UI;

namespace Gestion {
	
	public class MainController : MonoBehaviour {

		// Use this for initialization
		void Start () {

			for (int i = 0; i < 10; i++) {
				InstanciateFriend ();
			}

		}
		
		// Update is called once per frame
		void Update () {

			if (!LerpV)
				return;

			content_scroll_view.horizontalNormalizedPosition = Mathf.Lerp( 
				content_scroll_view.horizontalNormalizedPosition, 
				0.6f, 
				10*content_scroll_view.elasticity*Time.deltaTime);
			if(Mathf.Abs(content_scroll_view.horizontalNormalizedPosition - 0.6f) <= 0.05f)// Mathf.Approximately(content_scroll_view.horizontalNormalizedPosition, 0.6f)) 
				LerpV = false;         
		}


		// --- JUSTE POUR LE TEST

		public GameObject swungmen_panel;
		public ScrollRect content_scroll_view;
		public float actual_position;

		public bool LerpV = false;


		public void InstanciateFriend(){
			
			float scroll_view_w = content_scroll_view.content.sizeDelta.x;

			Transform panel = Instantiate (swungmen_panel).transform as Transform;


			float panel_w = ((RectTransform)panel).sizeDelta.x;

			RectTransform scroll_view = content_scroll_view.content.GetComponent<RectTransform> ();
			float new_scroll_view_w = scroll_view_w + (panel_w + 5);

			scroll_view.sizeDelta = new Vector2(new_scroll_view_w, scroll_view.sizeDelta.y);

			if (new_scroll_view_w > scroll_view_w) 
				scroll_view.anchoredPosition = new Vector3(new_scroll_view_w - scroll_view_w, 0);

			panel.transform.SetParent (content_scroll_view.content.transform, false);
			actual_position += ((panel_w / 2) + 5);
			((RectTransform)panel).anchoredPosition = new Vector2 (actual_position, 0);
			actual_position += ((panel_w / 2));


			Button buy = panel.Find ("Buy").GetComponent<Button> ();

			buy.onClick.AddListener(delegate() {
				OnSelectItem();
			});


		}



		void OnGUI()
		{
			float x = Input.mousePosition.x;
			float y = Input.mousePosition.y;
			GUI.Box(new Rect(0, 0, 500, 30), "Timer : " + x + " - " + y);
		}

		// Créer un ItemController ?
		public void OnSelectItem(){
			LerpV = true;
			Debug.Log (content_scroll_view.horizontalNormalizedPosition);
			Debug.Log ("Item selected");
		}

		// Créer un ScrollViewController ?
		public void OnScroll(){
			Debug.Log ("Scroll View");
		}

		// ---

	}
}