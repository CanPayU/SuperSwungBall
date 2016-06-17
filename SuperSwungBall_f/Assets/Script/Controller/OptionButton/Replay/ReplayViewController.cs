using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using Extension;

public class ReplayViewController : MonoBehaviour {

	private GameObject item;
	[SerializeField]
	private ScrollRect content_scroll_view;
	[SerializeField]
	private GameObject scroll_view;

	private float scroll_view_heigth;
	private float actual_position;

	// Use this for initialization
	void Start () {
		this.item = Resources.Load("Prefabs/OptionButton/Replay/Item") as GameObject;
		this.scroll_view_heigth = ((RectTransform)this.scroll_view.transform).sizeDelta.y;

		InstanciateItem ("Bonjour");
		InstanciateItem ("Comment vas-tu ?");
		InstanciateItem ("Je vais bien ! Merci !");

	}
	
	/// <summary>
	/// Ajoute un amis a la liste
	/// </summary>
	/// <param name="username">Username of friend</param>
	public void InstanciateItem(string name)
	{

		Transform panel = Instantiate(item).transform as Transform;
		Button btn = panel.GetComponent<Button>();
		btn.EditText (name);

		float panel_heigth = ((RectTransform)panel).sizeDelta.y;

		RectTransform scroll_view = content_scroll_view.content.GetComponent<RectTransform>();
		float new_scroll_view_heigth = scroll_view.sizeDelta.y + (panel_heigth + 5);

		scroll_view.sizeDelta = new Vector2(scroll_view.sizeDelta.x, new_scroll_view_heigth);

		if (new_scroll_view_heigth > this.scroll_view_heigth)
			scroll_view.anchoredPosition = new Vector3(0, new_scroll_view_heigth - this.scroll_view_heigth);

		panel.transform.SetParent(content_scroll_view.content.transform, false);
		actual_position -= ((panel_heigth / 2) + 5);
		((RectTransform)panel).anchoredPosition = new Vector2(0, actual_position);
		actual_position -= ((panel_heigth / 2));
	}
}
